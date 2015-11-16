using Redcat.Core.Communication;
using System;
using System.IO;
using System.Text;

namespace Redcat.Core.Net
{
    public abstract class NetworkChannel : MessageChannelBase
    {
        private INetworkStreamFactory factory;
        private Encoding encoding;
        private StreamProxy stream;
        
        public NetworkChannel(INetworkStreamFactory factory, ConnectionSettings settings) : base(settings)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            this.factory = factory;
        }

        protected Stream Stream
        {
            get { return stream; }
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            stream = new StreamProxy(factory.CreateStream(Settings));
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
            stream.OriginStream = factory.CreateSecuredStream(stream.OriginStream, Settings);
        }
    }
}
