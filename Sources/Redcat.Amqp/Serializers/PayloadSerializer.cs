using Redcat.Amqp.Performatives;
using System;
using System.Collections.Generic;
using System.IO;

namespace Redcat.Amqp.Serializers
{
    public class PayloadSerializer : IPayloadSerializer
    {
        private IDictionary<Type, PayloadWriter<object>> payloadWriters;        

        public PayloadSerializer()
        {
            payloadWriters = new Dictionary<Type, PayloadWriter<object>>();

            AddPayloadWriter<Open>(SerializeOpenPerformative);
            AddPayloadWriter<Close>(SerializeClosePerformative);
        }

        private void AddPayloadWriter<T>(PayloadWriter<T> payloadWriter)
        {
            var writer = new PayloadWriterAdapter<T>(payloadWriter);
            payloadWriters[typeof(T)] = writer.Write;
        }

        public void Serialize(Stream stream, object payload)
        {
            
        }

        public static void SerializeOpenPerformative(AmqpDataWriter writer, Open performative)
        { }

        public static void SerializeClosePerformative(AmqpDataWriter writer, Close performative)
        { }
    }

    internal class PayloadWriterAdapter<T>
    {
        private PayloadWriter<T> serializeAction;

        public PayloadWriterAdapter(PayloadWriter<T> serializeAction)
        {
            this.serializeAction = serializeAction;
        }

        public void Write(AmqpDataWriter writer, object payload)
        { }
    }

    internal delegate void PayloadWriter<T>(AmqpDataWriter writer, T payload);
}
