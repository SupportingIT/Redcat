using System;

namespace Redcat.Amqp.Serialization
{
    public abstract class PerformativeReaderBase<T> : IPayloadReader
    {
        private Action<T, AmqpDataReader>[] fieldInitializers;

        protected abstract T CreateDefault();

        protected abstract Action<T, AmqpDataReader>[] GetFieldInitializers();

        public object Read(AmqpDataReader reader)
        {
            if (!reader.IsList()) throw new InvalidOperationException();
            if (fieldInitializers == null) fieldInitializers = GetFieldInitializers();

            uint size, count;
            reader.ReadListSizeAndCount(out size, out count);
            T performative = CreateDefault();

            for(int i = 0; i < count; i++)
            {
                fieldInitializers[i](performative, reader);
            }

            return performative;
        }
    }
}
