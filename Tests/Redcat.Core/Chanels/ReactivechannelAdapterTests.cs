using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System.Threading.Tasks;
using System;

namespace Redcat.Core.Tests.Chanels
{
    [TestFixture]
    public class ReactiveChannelAdapterTests
    {
        [Test]
        public void Receive_Returns_Message_After_Received_Event()
        {
            var channel = new FakeChannel<string>();
            var adapter = new ReactiveChannelAdapter<string>(channel);
            string message = "Hello Moon";

            var task = Task.Run(() => adapter.Receive());
            channel.RiseReceivedEvent(message);

            Assert.That(task.Result, Is.EqualTo(message));
        }

        [Test]
        public void ReceiveAsync_Returns_Message_After_Received_Event()
        {
            var channel = new FakeChannel<string>();
            var adapter = new ReactiveChannelAdapter<string>(channel);
            string message = "Hello Mars";

            var task = adapter.ReceiveAsync();
            channel.RiseReceivedEvent(message);

            Assert.That(task.Result, Is.EqualTo(message));
        }
    }

    public class FakeChannel<T> : IReactiveInputChannel<T>
    {
        public ChannelState State => ChannelState.Open;        

        public void Close()
        { }

        public void Open()
        { }

        public void RiseReceivedEvent(T message)
        {
            Received?.Invoke(this, message);
        }

        public event EventHandler<T> Received;
        public event EventHandler<StateChangedEventArgs> StateChanged;
    }
}
