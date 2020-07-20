using System;
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
  public interface IEventBus
  {
    #region Methods
    /// <summary>
    ///   Publishes the specified message.
    /// </summary>
    /// <typeparam name="T">Type of the message</typeparam>
    /// <param name="e">The message object to publish</param>
    void Publish<T>(T e);

    Task PublishAsync<T>(T e);

    void Publish<T>(T e, object token);

    Task PublishAsync<T>(T e, object token);

    /// <summary>
    ///   Publishes the specified message.
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    /// <param name="e">The message object to publish</param>
    /// <param name="token">
    ///   With a token you can reuse the same type for other purpose, but enforce that only the once with the
    ///   right token will receive the message
    /// </param>
    IDisposable Subscribe<T>(Action<T> handler);

    IDisposable Subscribe<T>(Action<T> handler, object token);

    void UnSubscribe<T>(IDisposable eventBusObject);
    #endregion
  }
}