using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;

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
            IqStanza bindIq = Iq.BindSet();
            stream.Write(bindIq);
            var response = stream.Read();
            return false;
        }
    }
}
