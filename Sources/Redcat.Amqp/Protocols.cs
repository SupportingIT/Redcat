using System;

namespace Redcat.Amqp
{
    public static class Protocols
    {
        public static readonly ProtocolHeader AmqpV100 = new ProtocolHeader(ProtocolType.Amqp, new Version(1, 0, 0));
        public static readonly ProtocolHeader TlsV100 = new ProtocolHeader(ProtocolType.Tls, new Version(1, 0, 0));
        public static readonly ProtocolHeader SaslV100 = new ProtocolHeader(ProtocolType.Sasl, new Version(1, 0, 0));
    }
}
