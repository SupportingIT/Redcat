using System;
using System.Net.Sockets;
using Redcat.Core;
using Redcat.Core.Net;

namespace Redcat.Xmpp.Tests
{
    public class SocketAdapter : ISocket
    {
        private Socket socket;

        public SocketAdapter(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            this.socket = socket;
        }

        public int Available
        {
            get { return socket.Available; }
        }

        public bool IsConnected
        {
            get { return socket.Connected; }
        }

        public void Connect(ConnectionSettings settings)
        {
            socket.Connect(settings.Host, settings.Port);
        }

        public void Disconnect()
        {
            socket.Disconnect(true);
        }

        public int Receive(byte[] buffer, int offset, int size)
        {
            return socket.Receive(buffer, offset, size, SocketFlags.None);
        }

        public int Send(byte[] buffer, int offset, int size)
        {
            return socket.Send(buffer, offset, size, SocketFlags.None);
        }
    }
}
