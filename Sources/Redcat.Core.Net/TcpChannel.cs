using Redcat.Core.Channels;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Redcat.Core.Net
{
    public class TcpChannel : ChannelBase, IInputChannel<ArraySegment<byte>>, ISecureStreamChannel, IAsyncInputChannel<ArraySegment<byte>>, IObservable<ArraySegment<byte>>
    {
        private ICollection<IObserver<ArraySegment<byte>>> subscribers;

        private StreamProxy streamProxy;

        private byte[] buffer;
        private Socket socket;

        private SocketAsyncEventArgs args;
        private bool isListening;

        public TcpChannel(int bufferSize, ConnectionSettings settings) : base(settings)
        {
            subscribers = new List<IObserver<ArraySegment<byte>>>();
            buffer = new byte[bufferSize];
        }

        public bool AcceptAllCertificates { get; set; }

        protected override void OnOpening()
        {
            base.OnOpening();
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(Settings.Host, Settings.Port);
            streamProxy = new StreamProxy(new NetworkStream(socket));
            StartListen();
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            socket.Close();
        }

        public Stream GetStream()
        {
            if (streamProxy == null)
            {
                streamProxy = new StreamProxy(new NetworkStream(socket));
            }
            return streamProxy;
        }

        public void SetStreamSecurity()
        {
            SslStream sslStream = new SslStream(streamProxy.OriginStream, false, ValidateServerCertificate);
            sslStream.AuthenticateAsClient(Settings.Host);
        }

        public ArraySegment<byte> Receive()
        {
            int byteCount = streamProxy.Read(buffer, 0, buffer.Length);
            return new ArraySegment<byte>(buffer, 0, byteCount);
        }

        public async Task<ArraySegment<byte>> ReceiveAsync()
        {
            int byteCount = await streamProxy.ReadAsync(buffer, 0, buffer.Length);
            return new ArraySegment<byte>(buffer, 0, byteCount);
        }

        public void StartListen()
        {
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.BufferList = new List<ArraySegment<byte>> { new ArraySegment<byte>(new byte[0], 0, 0) };
                args.Completed += OnDataReceived;
            }
            isListening = true;
            socket.ReceiveAsync(args);
        }

        private void OnDataReceived(object sender, SocketAsyncEventArgs args)
        {                        
            subscribers.OnNext(Receive());
            if (isListening) socket.ReceiveAsync(args);
        }

        public IDisposable Subscribe(IObserver<ArraySegment<byte>> subscriber)
        {
            return subscribers.Subscribe(subscriber);
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
