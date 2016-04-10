using Redcat.Core.Serializaton;
using System;

namespace Redcat.Core.Channels
{
    public abstract class ReactiveChannelBase<TMessage, TSerializer, TDeserializer> : OutputChannelBase<TMessage, TSerializer> where TSerializer : ISerializer<TMessage> 
                                                                                      where TDeserializer : IReactiveDeserializer<TMessage>
    {
        private TDeserializer deserializer;

        protected ReactiveChannelBase(ConnectionSettings settings) : base(settings)
        { }                

        protected override void OnOpening()
        {
            base.OnOpening();
            InitializeDeserializer();
        }

        private void InitializeDeserializer()
        {
            if (deserializer == null)
            {
                deserializer = CreateDeserializer();
                deserializer.Deserialized += OnMessageDeserialized;
            }
        }

        protected abstract TDeserializer CreateDeserializer();

        private void OnMessageDeserialized(TMessage message)
        {
            Received?.Invoke(this, message);
        }

        public event EventHandler<TMessage> Received;
    }
}
