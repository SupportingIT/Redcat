using Redcat.Core.Channels;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;

namespace Redcat.Core.Net
{
    public class TcpChannel : ChannelBase, IStreamChannel, ISecureStreamChannel
    {
        private TcpClient tcpClient;

        public TcpChannel(ConnectionSettings settings) : base(settings)
        { }

        public bool AcceptAllCertificates { get; set; }

        protected override void OnOpening()
        {
            base.OnOpening();
            tcpClient = new TcpClient();
            tcpClient.Connect(Settings.Host, Settings.Port);
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            tcpClient.Close();
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

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (CertificateValidation != null)
            {
                CertificateValidationEventArgs args = new CertificateValidationEventArgs(certificate, chain, sslPolicyErrors);
                CertificateValidation(this, args);
                return args.ValidationResult;
            }

            if (AcceptAllCertificates) return true;

            return sslPolicyErrors == SslPolicyErrors.None;
        }

        public event EventHandler<CertificateValidationEventArgs> CertificateValidation;
    }

    public class CertificateValidationEventArgs : EventArgs
    {
        public CertificateValidationEventArgs(X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            Certificate = certificate;
            Chain = chain;
            Errors = errors;
        }

        public X509Certificate Certificate { get; }

        public X509Chain Chain { get; }

        public SslPolicyErrors Errors { get; }

        public bool ValidationResult { get; set; }
    }
}
