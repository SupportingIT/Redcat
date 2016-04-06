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

        private void OnMessageDeserialized(T message)
        {
            Received?.Invoke(this, message);
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            streamChannel.Close();            
        }

        public void Send(T message)
        {
            if (State != ChannelState.Open) throw new InvalidOperationException("Channel must be opend before sending messages");
            Serializer.Serialize(Stream, message);
        }

        protected void SetSecureStream()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<T> Received;
    }
}
