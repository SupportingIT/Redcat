using Redcat.Core.Communication;
using System.IO;
using System.Net.Sockets;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Redcat.Core.Net
{
    public class TcpChannel : ChannelBase, IStreamChannel, ISecureStreamChannel
    {
        private TcpClient tcpClient;

        public TcpChannel(ConnectionSettings settings) : base(settings)
        { }

        protected override void OnOpening()
        {
            base.OnOpening();
            tcpClient = new TcpClient();
            tcpClient.Connect(Settings.Host, Settings.Port);
        }

        public Stream GetStream()
        {
            return tcpClient.GetStream();
        }

        public Stream GetSecureStream()
        {
            SslStream secureStream = new SslStream(tcpClient.GetStream(), true, ValidateServerCertificate);
            secureStream.AuthenticateAsClient(Settings.Host);
            return secureStream;
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {            
            return true;
        }
    }
}
