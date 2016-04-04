using System;
using System.Collections.Generic;

namespace Redcat.Core.Channels
{
    public abstract class MessageReceiver<T>
    {
        private ByteBuffer buffer;
        private Queue<T> messageQueue;

        protected MessageReceiver(int bufferSize)
        {
            buffer = new ByteBuffer(bufferSize);
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

        public void OnBinaryDataReceived(ArraySegment<byte> binaryData)
        {
            buffer.Write(binaryData.Array, binaryData.Offset, binaryData.Count);
            OnBufferUpdated();
            while (messageQueue.Count > 0)
            {
                T message = messageQueue.Dequeue();
                OnMessageReceived(message);
            }
        }

        protected abstract void OnBufferUpdated();

        protected virtual void OnMessageReceived(T message)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<T> MessageReceived;
    }
}
