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
    public class TcpChannel : ObservableChannel<ArraySegment<byte>>, IInputChannel<ArraySegment<byte>>, ISecureStreamChannel, IAsyncInputChannel<ArraySegment<byte>>, IObservable<ArraySegment<byte>>
    {
        private Stream stream;

        private byte[] buffer;
        private Socket socket;

        private SocketAsyncEventArgs args;
        private bool isListening;

        public TcpChannel(int bufferSize, ConnectionSettings settings) : base(settings)
        {
            buffer = new byte[bufferSize];
        }

        public bool AcceptAllCertificates { get; set; }

        protected override void OnOpening()
        {
            base.OnOpening();
            if (socket == null) socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(Settings.Host, Settings.Port);
            stream = GetStream();            
            StartListen();
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            stream.Close();
            socket.Close();
            stream = null;
        }

        public Stream GetStream()
        {
            if (stream == null)
            {
                stream = new NetworkStream(socket);
            }
            return stream;
        }

        public Stream GetSecuredStream()
        {
            if (!(stream is SslStream))
            {
                StopListening();
                SslStream ssl = new SslStream(stream, true, ValidateServerCertificate);                
                ssl.AuthenticateAsClient(Settings.Host);
                stream = ssl;
                StartListen();
            }

            return stream;
        }

        public ArraySegment<byte> Receive()
        {
            int byteCount = stream.Read(buffer, 0, buffer.Length);
            return new ArraySegment<byte>(buffer, 0, byteCount);
        }

        public async Task<ArraySegment<byte>> ReceiveAsync()
        {
            int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            return new ArraySegment<byte>(buffer, 0, byteCount);
        }

        private void StartListen()
        {
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.BufferList = new List<ArraySegment<byte>> { new ArraySegment<byte>(new byte[0], 0, 0) };
                args.Completed += OnDataReceived;
            }
            isListening = true;
            DoReceive();
        }

        private void StopListening()
        {            
            isListening = false;
        }

        private void OnDataReceived(object sender, SocketAsyncEventArgs args)
        {
            if (!isListening) return;
            try
            {
                RiseOnNext(Receive());
            }
            catch (IOException e)
            {
                if (isListening && !socket.Connected) return;
                throw e;
            }
            DoReceive();
        }

        private void DoReceive()
        {
            if (!socket.ReceiveAsync(args))
            {
                OnDataReceived(socket, args);
            }
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

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();            
            args.Dispose();
            stream.Dispose();
            socket.Dispose();
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
