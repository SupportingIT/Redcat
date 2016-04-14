using System;
using Redcat.Core;

namespace Redcat.Amqp.Serialization
{
    public class PayloadReader : IPayloadReader
    {

        public object Deserialize(AmqpDataReader reader)
        {   
            if (reader.IsULongDescriptor())
            {
                ulong descriptor = reader.ReadULongDescriptor();
            }
            return null;
        }
    }
}
