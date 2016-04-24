using System.Collections.Generic;

namespace Redcat.Amqp.Serialization
{
    public class PayloadReader : IPayloadReader
    {
        private IDictionary<ulong, IPayloadReader> childReadersUlong = new Dictionary<ulong, IPayloadReader>();
        private IDictionary<string, IPayloadReader> childReadersStr = new Dictionary<string, IPayloadReader>();

        public void AddChildReader(ulong uDescriptor, string sDescriptor, IPayloadReader reader)
        {
            childReadersUlong[uDescriptor] = reader;
            if (!string.IsNullOrEmpty(sDescriptor)) childReadersStr[sDescriptor] = reader;
        }

        public object Read(AmqpDataReader reader)
        {
            if (reader.IsULongDescriptor())
            {
                ulong descriptor = reader.ReadULongDescriptor();
                IPayloadReader childReader = childReadersUlong[descriptor];
                return childReader.Read(reader);
            }
            //if (reader.IsS)
            return null;
        }
    }
}
