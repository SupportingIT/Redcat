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

        public bool CanNegotiate(NegotiationContext context, XmlElement feature)
        {
            return feature.Name == "starttls" 
                && feature.Xmlns == Namespaces.Tls
                && !context.IsTlsEstablished;
        }

        public bool Negotiate(NegotiationContext context, XmlElement feature)
        {
            context.Stream.Write(Tls.Start);
            var response = context.Stream.Read();
            if (IsResponseValid(response))
            {
                setTlsContext(); context.IsTlsEstablished = true;
            }
            else
            {
                context.Stream.Write(Tls.Failure);
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
