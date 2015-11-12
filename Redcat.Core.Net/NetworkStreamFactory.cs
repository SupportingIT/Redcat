using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Redcat.Core.Net
{
    public class NetworkStreamFactory : INetworkStreamFactory
    {
        private ISocketFactory socketFactory;

        public NetworkStreamFactory(ISocketFactory socketFactory)
        {
            if (socketFactory == null) throw new ArgumentNullException(nameof(socketFactory));
            this.socketFactory = socketFactory;
        }

        public Stream CreateStream(ConnectionSettings settings)
        {
            //ISocket socket = socketFactory.CreateSocket(settings);
            //socket.Connect(settings);
            //return new SocketStream(socket);
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(settings.Host, settings.Port);
            NetworkStream networkStream = new NetworkStream(socket);
            return networkStream;
        }
        public Stream CreateSecuredStream(Stream stream, ConnectionSettings settings)
        {
            SslStream securedStream = new SslStream(stream, false, ValidateServerCertificate);
            securedStream.AuthenticateAsClient(settings.Host);
            return securedStream;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            throw new NotImplementedException();
        }
    }
}
