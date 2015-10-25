using System;
using System.IO;
using System.Text;

namespace Redcat.Core.Net
{
    public abstract class NetworkChannel : MessageChannelBase
    {
        private Encoding encoding;
        private ISocket socket;

        protected NetworkChannel(ISocket socket, ConnectionSettings settings) : base(settings)
        {
            this.socket = socket;
        }

        public Func<Stream, Stream> TlsContextFactory { get; set; }

        protected ISocket Socket
        {
            get { return socket; }
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            Socket.Connect(Settings);
        }

        protected void Send(string data)
        {
            byte[] buffer = encoding.GetBytes(data);
            Send(buffer);
        }

        protected void Send(byte[] buffer)
        {                      
            Send(buffer, 0, buffer.Length);
        }

        protected void Send(byte[] buffer, int offset, int count)
        {
            Socket.Send(buffer, offset, count);
        }

        protected void SetTlsContext()
        {
            if (TlsContextFactory == null) throw new InvalidOperationException();
            SocketStream stream = new SocketStream(socket);
            Stream tlsStream = TlsContextFactory.Invoke(stream);
        }
    }
}
