using System;
using System.IO;
using System.Text;

namespace Redcat.Core.Net
{
    public abstract class NetworkChannel : MessageChannelBase
    {
        private INetworkStreamFactory factory;
        private Encoding encoding;
        private Stream stream;
        
        public NetworkChannel(INetworkStreamFactory factory, ConnectionSettings settings) : base(settings)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.factory = factory;
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            stream = factory.CreateStream(Settings);
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            stream.Dispose();
        }

        protected void Send(string data)
        {
            byte[] buffer = encoding.GetBytes(data);
            Send(buffer);
        }

        public void Send(byte[] buffer)
        {                      
            Send(buffer, 0, buffer.Length);
        }

        public void Send(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (stream == null) throw new InvalidOperationException();
            stream.Write(buffer, offset, count);
        }

        public void SetSecuredStream()
        {
            if (stream == null) throw new InvalidOperationException();
            stream = factory.CreateSecuredStream(stream, Settings);
        }
    }
}
