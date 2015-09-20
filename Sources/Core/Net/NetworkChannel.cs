using System;
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
    }
}
