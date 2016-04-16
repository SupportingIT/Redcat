using System.Collections.Generic;

namespace Redcat.Amqp.Serialization
{
    public class PayloadReader : IPayloadReader
    {
        private IDictionary<ulong, IPayloadReader> childReaders = new Dictionary<ulong, IPayloadReader>();        

        public void AddChildReader(ulong uDescriptor, string sDescriptor, IPayloadReader reader)
        {
            childReaders[uDescriptor] = reader;
        }

        public object Read(AmqpDataReader reader)
        {
            if (reader.IsULongDescriptor())
            {
                ulong descriptor = reader.ReadULongDescriptor();
                IPayloadReader childReader = childReaders[descriptor];
                return childReader.Read(reader);
            }
            return null;
        }
    }
}
