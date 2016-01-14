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

        public bool Negotiate(IXmppStream stream, XmlElement feature)
        {
            IqStanza register = Iq.Get();
            register.Id = 8;
            register.To = "redcat";
            register.AddChild(new XmlElement("query", "jabber:iq:register"));
            stream.Write(register);
            var response = stream.Read();
            return false;
        }
    }
}
