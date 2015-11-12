using System.Net.Sockets;

namespace Redcat.Core.Net
{
    public class SocketFactory : ISocketFactory
    {
        public ISocket CreateSocket(ConnectionSettings settings)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);            
            return new SocketAdapter(socket);
        }
    }
}
