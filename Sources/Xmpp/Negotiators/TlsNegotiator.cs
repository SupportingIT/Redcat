using System;
using Redcat.Xmpp.Xml;
using System.Net;

namespace Redcat.Xmpp.Negotiators
{
    public class TlsNegotiator : IFeatureNegatiator
    {
        private Action setTlsContext;

        public TlsNegotiator(Action setTlsContext)
        {
            if (setTlsContext == null) throw new ArgumentNullException(nameof(setTlsContext));
            this.setTlsContext = setTlsContext;
        }

        public bool CanNeogatiate(XmlElement feature)
        {
            return feature.Name == "starttls" && feature.Xmlns == Namespaces.Tls;
        }

        public bool Neogatiate(IXmppStream stream, XmlElement feature)
        {
            stream.Write(Tls.Start);
            var response = stream.Read();
            if (IsResponseValid(response)) setTlsContext();
            else
            {
                stream.Write(Tls.Failure);
                throw new ProtocolViolationException("Invalid response received from server");
            }
            return true;
        }

        private bool IsResponseValid(XmlElement response)
        {
            return response.Name == "proceed" && response.Xmlns == Namespaces.Tls;
        }
    }
}
