using System;
using System.IO;

namespace Redcat.Core.Channels
{
    public abstract class ReactiveChannelBase<T> : ChannelBase, IOutputChannel<T>
    {
        private IReactiveDeserializer<T> deserializer;
        private ISerializer<T> serializer;
        private IReactiveStreamChannel streamChannel;
        private Stream stream;

        protected ReactiveChannelBase(IReactiveStreamChannel streamChannel, ConnectionSettings settings) : base(settings)
        {
            this.streamChannel = streamChannel;
        }

        protected IReactiveDeserializer<T> Deserializer => deserializer;

        protected abstract IReactiveDeserializer<T> CreateDeserializer();

        protected ISerializer<T> Serializer
        {
            get
            {
                if (serializer == null) serializer = CreateSerializer();
                return serializer;
            }
        }

        protected abstract ISerializer<T> CreateSerializer();

        protected Stream Stream
        {
            get
            {
                if (stream == null) stream = streamChannel.GetStream();
                return stream;
            }
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            streamChannel.Open();
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            streamChannel.Close();            
        }

        public void Send(T message)
        {
            Serializer.Serialize(Stream, message);
        }

        public event EventHandler<T> Received;
    }
}
