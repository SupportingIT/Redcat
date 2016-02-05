using Redcat.Xmpp.Xml;
using System;
using System.Net;

namespace Redcat.Xmpp.Negotiators
{
    public class BindNegotiator : IFeatureNegatiator
    {
        public bool CanNegotiate(NegotiationContext context, XmlElement feature)
        {
            if (feature == null) throw new ArgumentNullException(nameof(feature));
            if (context.Jid != null) return false;
            return feature.Name == "bind" && feature.Xmlns == Namespaces.Bind;
        }

        public bool Negotiate(NegotiationContext context, XmlElement feature)
        {
            IqStanza bindIq = CreateBindRequest();
            context.Stream.Write(bindIq);
            JID response = ReadUserJid(context.Stream);
            context.Jid = response;
            return false;
        }

        private IqStanza CreateBindRequest()
        {
            IqStanza stanza = Iq.Set();
            stanza.AddChild(Bind.New());            
            return stanza;
        }

        private JID ReadUserJid(IXmppStream stream)
        {
            XmlElement response = stream.Read();
            VerifyBindResponse(response);
            response = response.Child("bind").Child("jid");
            return response.Value.ToString();
        }

        private void VerifyBindResponse(XmlElement response)
        {
            if (!response.HasChild("bind")) throw new ProtocolViolationException();
            if (!response.Child("bind").HasChild("jid")) throw new ProtocolViolationException();
        }
    }
}
