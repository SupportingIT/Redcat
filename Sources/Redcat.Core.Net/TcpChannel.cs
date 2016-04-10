using Redcat.Core.Channels;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;

namespace Redcat.Core.Net
{
    public class TcpChannel : ChannelBase, IReactiveStreamChannel
    {
        private Stream stream;

        private byte[] buffer;
        private Socket socket;

        private SocketAsyncEventArgs args;
        private bool isListening = false;

        public TcpChannel(int bufferSize, ConnectionSettings settings) : base(settings)
        {
            args = new SocketAsyncEventArgs();
            args.SetBuffer(new byte[1], 0, 1);
            args.Completed += OnReceiveCompleted;

            buffer = new byte[bufferSize];
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            int readed = stream.Read(buffer, 0, buffer.Length);
            Received?.Invoke(this, new ArraySegment<byte>(buffer, 0, readed));
            if (isListening) ReceiveAsync();
        }

        public bool AcceptAllCertificates { get; set; }

        protected override void OnOpening()
        {
            base.OnOpening();
            socket.Connect(Settings.Host, Settings.Port);
            StartListening();
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            socket.Close();
        }

        public Stream GetStream()
        {
            if (stream == null) stream = new NetworkStream(socket);
            return stream;
        }

        public Stream GetSecuredStream()
        {
            if (!(stream is SslStream))
            {
                SslStream ssl = new SslStream(stream, false, ValidateServerCertificate);
                ssl.AuthenticateAsClient(Settings.Host);
                stream = ssl;
            }

            return stream;
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

        public void ReceiveAsync()
        {
            socket.ReceiveAsync(args);
        }
        
        public void StartListening()
        {
            isListening = true;
            ReceiveAsync();
        }

        public void StopListening()
        {
            isListening = false;
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            args.Dispose();
            stream.Dispose();
            socket.Dispose();
        }

        public event EventHandler<CertificateValidationEventArgs> CertificateValidation;
        public event EventHandler<ArraySegment<byte>> Received;
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