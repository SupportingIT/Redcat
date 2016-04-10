using System;

namespace Redcat.Amqp
{
    public class ProtocolHeader
    {
        public ProtocolHeader(ProtocolType protocolType, Version version)
        {
            ProtocolType = protocolType;
            Version = version;
        }

        public ProtocolType ProtocolType { get; }
        public Version Version { get; }

        public override bool Equals(object obj)
        {
            ProtocolHeader header = obj as ProtocolHeader;
            if (obj == null) return false;
            return ProtocolType == header.ProtocolType && Version == header.Version;
        }
    }

    public enum ProtocolType { Amqp = 0, Tls = 2, Sasl = 3 }
}
