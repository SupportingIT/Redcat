using Redcat.Amqp.Performatives;
using System;
using System.Collections.Generic;

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

        public void Serialize(AmqpDataWriter writer, object payload)
        {
            if (!payloadWriters.ContainsKey(payload.GetType())) throw new InvalidOperationException();
            var payloadWriter = payloadWriters[payload.GetType()];            
            payloadWriter.Invoke(writer, payload);
        }

        public static void SerializeOpenPerformative(AmqpDataWriter writer, Open performative)
        {                        
            SerializeComposite(writer, Descriptors.Open, performative.ContainerId,
                                                         performative.Hostname,
                                                         performative.MaxFrameSize,
                                                         performative.MaxChannel,
                                                         performative.IdleTimeout);
        }

        public static void SerializeClosePerformative(AmqpDataWriter writer, Close performative)
        {
            SerializeComposite(writer, Descriptors.Close, performative.Error);
        }

        private static void SerializeComposite(AmqpDataWriter writer, string descriptor, params object[] list)
        {
            writer.WriteDescriptor(descriptor);
            writer.WriteList(list);
        }
    }

    internal class PayloadWriterAdapter<T>
    {
        private PayloadWriter<T> serializeAction;

        public PayloadWriterAdapter(PayloadWriter<T> serializeAction)
        {
            this.serializeAction = serializeAction;
        }

        public void Write(AmqpDataWriter writer, object payload)
        {
            serializeAction(writer, (T)payload);
        }
    }

    internal delegate void PayloadWriter<T>(AmqpDataWriter writer, T payload);
}
