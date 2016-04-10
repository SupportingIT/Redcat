using Redcat.Core.Serialization;
using System;
using System.IO;

namespace Redcat.Core.Channels
{
    public abstract class ReactiveMessageChannel<TMessage, TSerializer, TDeserializer> : ReactiveChannelBase<TMessage, TSerializer, TDeserializer> where TSerializer : ISerializer<TMessage>
                                                                                      where TDeserializer : IReactiveDeserializer<TMessage>
    {
        private IReactiveStreamChannel transportChannel;
        private Stream stream;

        public ReactiveMessageChannel(IReactiveStreamChannel transportChannel, ConnectionSettings settings) : base(settings)
        {
            this.transportChannel = transportChannel;
            this.transportChannel.Received += OnBinaryDataReceived;
        }

        protected override Stream Stream
        {
            get
            {
                if (stream == null) stream = transportChannel.GetStream();
                return stream;
            }
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            transportChannel.Open();            
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            transportChannel.Close();
            stream = null;
        }

        protected void SetSecureTransport()
        {
            if (transportChannel is ISecureStreamChannel) stream = ((ISecureStreamChannel)transportChannel).GetSecuredStream();
            else throw new InvalidOperationException("Transport channel does not support channel security");
        }

        private void OnBinaryDataReceived(object sender, ArraySegment<byte> data)
        {
            Deserializer.Read(data);
        }
    }
}
