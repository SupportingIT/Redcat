using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Net;

namespace Redcat.Xmpp.Negotiators
{
    public class BindNegotiator : IFeatureNegatiator
    {
        private ConnectionSettings settings;

        public BindNegotiator(ConnectionSettings settings)
        {
            this.settings = settings;
        }

        public bool CanNegotiate(XmlElement feature)
        {
            if (feature == null) throw new ArgumentNullException(nameof(feature));
            return feature.Name == "bind" && feature.Xmlns == Namespaces.Bind;
        }

        public bool Negotiate(IXmppStream stream, XmlElement feature)
        {
            IqStanza bindIq = CreateBindRequest();
            stream.Write(bindIq);
            JID response = ReadUserJid(stream);
            settings.UserJid(response);
            return true;
        }

        private IqStanza CreateBindRequest()
        {
            IqStanza stanza = Iq.Set();
            string resource = settings.Resource();
            if (string.IsNullOrEmpty(resource)) stanza.AddChild(Bind.New());
            else stanza.AddChild(Bind.Resource(resource));
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
