using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Redcat.Core.Channels
{
    public class BufferChannel<T> : ObservableChannel<T>, IInputChannel<T>, IAsyncInputChannel<T>, IObservable<T>, IObserver<ArraySegment<byte>>
    {
        private ByteBuffer buffer;
        private Queue<T> messageQueue;
        private ManualResetEventSlim bufferEvent;

        public BufferChannel(int bufferSize, ConnectionSettings settings) : base(settings)
        {
            buffer = new ByteBuffer(bufferSize);
            bufferEvent = new ManualResetEventSlim(false);
            messageQueue = new Queue<T>();
        }

        protected ByteBuffer Buffer => buffer;

        protected void EnqueueMessage(T message)
        {
            messageQueue.Enqueue(message);
        }

        protected void EnqueueMessages(IEnumerable<T> messages)
        {
            foreach (T message in messages) EnqueueMessage(message);
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(ArraySegment<byte> value)
        {
            lock (messageQueue)
            {
                buffer.Write(value.Array, value.Offset, value.Count);
                OnBufferUpdated();
                if (messageQueue.Count > 0)
                {
                    T message = messageQueue.Peek();
                    OnMessageReceived(message);
                    RiseOnNext(message);
                    if (!bufferEvent.IsSet) bufferEvent.Set();
                }
            }
        }

        protected virtual void OnBufferUpdated()
        { }

        public T Receive()
        {
            return RetreiveMessage();
        }

        public async Task<T> ReceiveAsync()
        {
            return await Task.Run(() => RetreiveMessage());
        }

        private T RetreiveMessage()
        {
            if (messageQueue.Count == 0)
            {
                bufferEvent.Wait();
            }

            lock (messageQueue)
            {
                if (messageQueue.Count == 1) bufferEvent.Reset();
                T message = messageQueue.Dequeue();
                OnMessageReceived(message);
                RiseOnNext(message);
                return message;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            bufferEvent.Set();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            bufferEvent.Dispose();
        }
    }
}
