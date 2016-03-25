using Redcat.Amqp.Serializers;
using Redcat.Core;
using Redcat.Core.Channels;
using System;
using System.IO;
using System.Linq;

namespace Redcat.Amqp.Channels
{
    public class AmqpChannel : BufferChannel<AmqpFrame>, IAmqpChannel
    {        
        private readonly byte[] amqpHeader = { (byte)'A', (byte)'M', (byte)'Q', (byte)'P', 0, 1, 0, 0 };

        private AmqpFrameSerializer serializer;
        private IStreamChannel streamChannel;
        private Stream stream;

        public AmqpChannel(IStreamChannel streamChannel, ConnectionSettings settings, int bufferSize) : base(bufferSize, settings)
        {
            this.streamChannel = streamChannel;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            streamChannel.Open();
            stream = streamChannel.GetStream();
            serializer = new AmqpFrameSerializer(stream, new PayloadSerializer());
            InitializeChannel();
        }

        private void InitializeChannel()
        {
            byte[] response = new byte[8];
            stream.Write(amqpHeader, 0, amqpHeader.Length);
            stream.Read(response, 0, 8);
            if (!IsValidAmqpHeader(response)) throw new InvalidOperationException();
        }

        private bool IsValidAmqpHeader(byte[] header)
        {
            return amqpHeader.SequenceEqual(header);
        }

        protected override void OnBufferUpdated()
        {
            base.OnBufferUpdated();
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            streamChannel.Close();
        }

        public void Send(AmqpFrame frame)
        {
            serializer.Serialize(frame);
        }
    }
}
