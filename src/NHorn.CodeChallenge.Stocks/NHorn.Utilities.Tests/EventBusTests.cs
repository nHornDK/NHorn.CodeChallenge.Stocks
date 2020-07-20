using NUnit.Framework;
using System;
using NHorn.Utilities;
namespace NHorn.Utilities.Tests
{
    [TestFixture]
    public class EventBusTests
    {
        private string _okMsg;
        private string _notOkMsg;
        private int _unsubscribeIteration;

        private void ReceiveOkMessage(MyOkMessage message)
        {
            _okMsg = message.Message;
        }

        private void ReceiveMyUnsubscribeMessage(MyUnsubscribeMessage message)
        {
            _unsubscribeIteration += 1;
        }

        private void ReceiveNotOkMessage(MyNotOkMessage message)
        {
            _notOkMsg = message.Message;
        }

        [Test]
        public void EventBusFlowWithNoTokenAndWrongTypeTest()
        {
            var eventBus = new EventBus();
            var myMessage = new MyOkMessage { Message = "not_ok_message" };
            eventBus.Subscribe<MyNotOkMessage>(ReceiveNotOkMessage);
            eventBus.Publish(myMessage);
            Assert.AreNotEqual(myMessage.Message, _notOkMsg);
        }

        [Test]
        public void EventBusFlowWithNoTokenTest()
        {
            var eventBus = new EventBus();
            var myMessage = new MyOkMessage { Message = "ok_message" };
            eventBus.Subscribe<MyOkMessage>(ReceiveOkMessage);
            eventBus.Publish(myMessage);
            Assert.AreEqual(myMessage.Message, _okMsg);
        }

        [Test]
        public void EventBusFlowWithTokenTest()
        {
            var eventBus = new EventBus();
            var myMessage = new MyOkMessage { Message = "ok_message" };
            eventBus.Subscribe<MyOkMessage>(ReceiveOkMessage, "token");
            eventBus.Publish(myMessage, "token");
            Assert.AreEqual(myMessage.Message, _okMsg);
        }

        [Test]
        public void EventBusFlowWithWrongTokenTest()
        {
            string okMsg = "";
            var eventBus = new EventBus();
            var myMessage = new MyOkMessage { Message = "ok_message" };
            eventBus.Subscribe<MyOkMessage>(m => okMsg = m.Message, "token1");
            eventBus.Publish(myMessage, "token");
            Assert.AreNotEqual(myMessage.Message, okMsg);
        }

        [Test]
        public void EventBusUnsubscribe()
        {
            var eventBus = new EventBus();
            _unsubscribeIteration = 0;
            var myMessage = new MyUnsubscribeMessage { Message = "Unsubscribe" };
            var myEvent = eventBus.Subscribe<MyUnsubscribeMessage>(ReceiveMyUnsubscribeMessage, "token");
            eventBus.Publish(myMessage, "token");
            eventBus.Publish(myMessage, "token");
            eventBus.Publish(myMessage, "token");
            eventBus.UnSubscribe<MyUnsubscribeMessage>(myEvent);
            eventBus.Publish(myMessage, "token");
            Assert.AreEqual(_unsubscribeIteration, 3);
        }

        [Test]
        public void NestedPublishUnsubscribe()
        {
            var eventBus = new EventBus();
            _unsubscribeIteration = 0;
            var myMessage = new MyUnsubscribeMessage { Message = "Unsubscribe" };
            IDisposable myEvent = null;
            // Unsubscribe as part of handler
            Action<MyUnsubscribeMessage> handler = m => { eventBus.UnSubscribe<MyUnsubscribeMessage>(myEvent); };
            myEvent = eventBus.Subscribe(handler, "token");
            eventBus.Publish(myMessage, "token");
            Assert.Pass("No exceptions");
        }
    }

    public class MyOkMessage
    {
        #region Properties
        public string Message { get; set; }
        #endregion
    }

    public class MyUnsubscribeMessage
    {
        #region Properties
        public string Message { get; set; }
        #endregion
    }

    public class MyNotOkMessage
    {
        #region Properties
        public string Message { get; set; }
        #endregion
    }
}
