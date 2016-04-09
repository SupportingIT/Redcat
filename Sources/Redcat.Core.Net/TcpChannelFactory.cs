using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Net
{
    public class TcpChannelFactory : IChannelFactory<IReactiveStreamChannel>
    {
        private const int DefaultBufferSize = 1024;

        public bool AcceptAllCertificates { get; set; }

        public int BufferSize { get; set; } = DefaultBufferSize;

        public EventHandler<CertificateValidationEventArgs> CertificateValidator { get; set; }

        public IReactiveStreamChannel CreateChannel(ConnectionSettings settings)
        {
            TcpChannel channel = new TcpChannel(BufferSize, settings);
            channel.AcceptAllCertificates = AcceptAllCertificates;
            if (CertificateValidator != null) channel.CertificateValidation += CertificateValidator;
            return channel;
        }
    }
}
