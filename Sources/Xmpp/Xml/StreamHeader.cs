using System;

namespace Redcat.Xmpp.Xml
{
    public class StreamHeader : DatagramElement
    {
        private StreamHeader(string xmlns) : base("stream:stream", xmlns)
        {
            SetAttributeValue("version", "1.0");
            SetAttributeValue("xmlns:stream", Namespaces.Streams);
        }

        public static StreamHeader CreateClientHeader(JID to)
        {
            return new StreamHeader(Namespaces.JabberClient)
            {
                To = to
            };
        }
    }
}
