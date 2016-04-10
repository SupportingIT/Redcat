using Redcat.Core.Serialization;
using System;
using System.IO;

namespace Redcat.Core.Channels
{
    public abstract class OutputChannelBase<TMessage, TSerializer> : ChannelBase, IOutputChannel<TMessage> where TSerializer : ISerializer<TMessage>
    {
        private TSerializer serializer;

        protected OutputChannelBase(ConnectionSettings settings) : base(settings)
        { }

        protected TSerializer Serializer
        {
            get
            {
                if (serializer == null) serializer = CreateSerializer();
                return serializer;
            }
        }

        protected abstract TSerializer CreateSerializer();

        protected abstract Stream Stream { get; }

        public void Send(TMessage message)
        {
            if (State != ChannelState.Open) throw new InvalidOperationException("Channel must be opend before sending messages");
            Serializer.Serialize(Stream, message);
        }
    }
}
