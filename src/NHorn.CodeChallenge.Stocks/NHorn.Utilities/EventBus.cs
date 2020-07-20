using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NHorn.Utilities
{
  /// <summary>
  ///   A message broker that transports messages between subscribers and publisher
  /// </summary>
  /// <remarks>
  ///   The event bus core functionality is to separate dependencies between classes, like if you need to hook up to an
  ///   even
  ///   on another class, you will need to create an instance of that class and then hook up to the event, you don't need
  ///   to
  ///   know anything about other classes with the event bus.
  /// </remarks>
  /// <seealso cref="IEventBus" />
  public class EventBus : IEventBus
  {
    #region Fields
    private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
    //private readonly ConcurrentDictionary<Type, List<KeyValuePair<object, Action<object>>>> handlers =
    //  new ConcurrentDictionary<Type, List<KeyValuePair<object, Action<object>>>>();
    private readonly ConcurrentDictionary<Type, List<IDisposable>> handlers =
      new ConcurrentDictionary<Type, List<IDisposable>>();
    #endregion

    #region Methods
    /// <summary>
    ///   Publishes the specified message.
    /// </summary>
    /// <typeparam name="T">Type of the message</typeparam>
    /// <param name="e">The message object to publish</param>
    public void Publish<T>(T e)
    {
      Publish(e, null);
    }

    public Task PublishAsync<T>(T e)
    {
      return Task.Run(() =>
        Publish(e));
    }

    /// <summary>
    ///   Publishes the specified message.
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    /// <param name="e">The message object to publish</param>
    /// <param name="token">
    ///   With a token you can reuse the same type for other purpose, but enforce that only the once with the
    ///   right token will receive the message
    /// </param>
    public void Publish<T>(T e, object token)
    {
      var busObjects = new List<EventBusObject>();
      var eventType = typeof(T);
      rwLock.EnterReadLock();
      foreach (var handlerType in handlers.Keys)
      {
        if (handlerType.IsAssignableFrom(eventType) && handlers.ContainsKey(eventType))
        {
          foreach (var disposable in handlers[eventType])
          {
            var eventBusObj = (EventBusObject) disposable;
            if (eventBusObj.Token == token)
            {
              busObjects.Add(eventBusObj);
            }
          }
        }
      }

      rwLock.ExitReadLock();
      busObjects.ForEach(x => x.Method(e));
    }

    public Task PublishAsync<T>(T e, object token)
    {
      return Task.Run(() =>
        Publish(e, token));
    }

    /// <summary>
    ///   Subscribes to the specified message.
    /// </summary>
    /// <typeparam name="T">Type of the message</typeparam>
    /// <param name="handler">
    ///   Method to execute when a publish is performed, remember that the message signature must match the
    ///   type.
    /// </param>
    public IDisposable Subscribe<T>(Action<T> handler)
    {
      return Subscribe(handler, null);
    }

    /// <summary>
    ///   Subscribes to the specified message.
    /// </summary>
    /// <typeparam name="T">MEssage type</typeparam>
    /// <param name="handler">
    ///   Method to execute when a publish is performed, remember that the message signature must match the
    ///   type.
    /// </param>
    /// <param name="token">
    ///   With a token you can reuse the same type for other purpose, but enforce that only the once with the
    ///   right token will receive the message
    /// </param>
    public IDisposable Subscribe<T>(Action<T> handler, object token)
    {
      rwLock.EnterWriteLock();
      if (!handlers.Any(t => t.Key == typeof(T) && t.Value.Any(to => ((EventBusObject) to).Token == token)))
      {
        handlers.TryAdd(typeof(T), new List<IDisposable>());
      }

      var eventBusObject = new EventBusObject {Method = x => handler((T) x), Token = token};
      handlers[typeof(T)].Add(eventBusObject);
      rwLock.ExitWriteLock();
      return eventBusObject;
    }

    public void UnSubscribe<T>(IDisposable eventBusObject)
    {
      rwLock.EnterWriteLock();
      if (handlers.ContainsKey(typeof(T)) && handlers[typeof(T)].Any(x => x.Equals(eventBusObject)))
      {
        handlers[typeof(T)].Remove(eventBusObject);
      }

      rwLock.ExitWriteLock();
    }

    private class EventBusObject : IDisposable
    {
      #region Properties
      public Action<object> Method { get; set; }
      public object Token { get; set; }
      #endregion

      #region Methods
      public void Dispose()
      {
      }
      #endregion
    }
    #endregion
  }
}