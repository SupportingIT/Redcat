using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Negotiators
{
    public class SessionNegotiator : IFeatureNegatiator
    {
        public bool CanNegotiate(NegotiationContext context, XmlElement feature)
        {
            return feature.Xmlns == Namespaces.Session && context.Jid != null && !context.IsSessionEsteblished;
        }

        public bool Negotiate(NegotiationContext context, XmlElement feature)
        {
            IqStanza sessionRequest = Iq.Set();
            sessionRequest.AddChild(new XmlElement("session", Namespaces.Session));            
            context.Stream.Write(sessionRequest);
            var response = context.Stream.Read();
            if (response.Name == "iq" && response.GetAttributeValue<string>("type") == "result") context.IsSessionEsteblished = true;
            return false;
        }
    }
}
