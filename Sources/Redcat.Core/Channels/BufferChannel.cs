using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Redcat.Core.Channels
{
    public class BufferChannel<T> : ChannelBase, IInputChannel<T>, IAsyncInputChannel<T>, IObservable<T>, IObserver<ArraySegment<byte>>
    {
        private ByteBuffer buffer;
        private ICollection<IObserver<T>> observers;
        private Queue<T> messageQueue;
        private ManualResetEventSlim bufferEvent;

        public BufferChannel(int bufferSize, ConnectionSettings settings) : base(settings)
        {
            buffer = new ByteBuffer(bufferSize);
            bufferEvent = new ManualResetEventSlim(false);
            messageQueue = new Queue<T>();
            observers = new List<IObserver<T>>();
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
                if (messageQueue.Count > 0 && !bufferEvent.IsSet) bufferEvent.Set();
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
                return messageQueue.Dequeue();
            }
        }        

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return observers.Subscribe(observer);
        }
    }
}
