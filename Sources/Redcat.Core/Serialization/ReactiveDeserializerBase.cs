using System;

namespace Redcat.Core.Serializaton
{
    public abstract class ReactiveDeserializerBase<T> : IReactiveDeserializer<T>
    {
        private ByteBuffer buffer;

        protected ReactiveDeserializerBase(int bufferSize)
        {
            buffer = new ByteBuffer(bufferSize);            
        }

        protected ByteBuffer Buffer => buffer;

        public void Read(ArraySegment<byte> binaryData)
        {
            buffer.Write(binaryData.Array, binaryData.Offset, binaryData.Count);
            OnBufferUpdated();
        }

        protected abstract void OnBufferUpdated();

        protected virtual void OnDeserialized(T message)
        {
            Deserialized?.Invoke(message);
        }

        public event Action<T> Deserialized;
    }
}
