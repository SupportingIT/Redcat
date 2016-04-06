using System.Threading;
using System.Threading.Tasks;

namespace Redcat.Core.Channels
{
    public class ReactiveChannelAdapter<T>
    {
        private IReactiveInputChannel<T> reactiveChannel;
        private ManualResetEventSlim resetEvent;
        private T receivedMessage;

        public ReactiveChannelAdapter(IReactiveInputChannel<T> reactiveChannel)
        {
            this.reactiveChannel = reactiveChannel;
            this.reactiveChannel.Received += OnMessageReceived;
            resetEvent = new ManualResetEventSlim();
        }

        public T Receive()
        {
            resetEvent.Wait();
            resetEvent.Reset();
            return receivedMessage;            
        }

        public Task<T> ReceiveAsync()
        {
            return Task.Run(() => Receive());
        }

        private void OnMessageReceived(object sender, T message)
        {
            receivedMessage = message;
            resetEvent.Set();
        }
    }
}
