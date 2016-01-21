using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Negotiators
{
    public class RegistrationNegotiator : IFeatureNegatiator
    {
        public bool CanNegotiate(XmlElement feature)
        {
            return feature.Name == "register";
        }

        public bool Negotiate(NegotiationContext context)
        {
            IqStanza register = Iq.Get();
            register.AddChild(new XmlElement("query", "jabber:iq:register"));
            context.Stream.Write(register);
            var response = context.Stream.Read();
            return false;
        }
    }
}
