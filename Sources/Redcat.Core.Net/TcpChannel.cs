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
    public class TcpChannel : ChannelBase, IStreamChannel, ISecureStreamChannel, IAsyncInputChannel<ArraySegment<byte>>, IObservable<ArraySegment<byte>>
    {
        private ICollection<IObserver<ArraySegment<byte>>> subscribers;

        private NetworkStream stream;
        private SslStream secureStream;

        private byte[] buffer;
        private Socket socket;

        private SocketAsyncEventArgs args;
        private TaskCompletionSource<ArraySegment<byte>> complitionSource;

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
            ReceiveAsync();
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

        public Stream GetSecureStream()
        {
            if (secureStream == null)
            {
                secureStream = new SslStream(stream, true, ValidateServerCertificate);
                secureStream.AuthenticateAsClient(Settings.Host);
            }
            return secureStream;
        }

        public ArraySegment<byte> Receive()
        {
            int receivedBytes = socket.Receive(buffer);
            return new ArraySegment<byte>(buffer, 0, receivedBytes);
        }

        public async Task<ArraySegment<byte>> ReceiveAsync()
        {
            if (args == null)
            {
                complitionSource = new TaskCompletionSource<ArraySegment<byte>>();
                args = new SocketAsyncEventArgs();
                args.SetBuffer(buffer, 0, buffer.Length);                
                args.Completed += OnDataReceived;
            }

            if (!socket.ReceiveAsync(args))
            {
                return new ArraySegment<byte>(args.Buffer, args.Offset, args.Count);
            }
            
            return await complitionSource.Task;            
        }

        private void OnDataReceived(object sender, SocketAsyncEventArgs args)
        {            
            var result = new ArraySegment<byte>(buffer, 0, args.BytesTransferred);
            complitionSource.SetResult(result);
            subscribers.OnNext(result);
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
