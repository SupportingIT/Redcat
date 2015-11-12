using System;
using System.Net.Sockets;

namespace Redcat.Core.Net
{
    public class SocketAdapter : ISocket
    {
        private Socket socket;

        public SocketAdapter(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
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
