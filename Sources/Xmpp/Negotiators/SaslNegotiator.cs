using System;
using Redcat.Xmpp.Xml;
using System.Text;
using Redcat.Core;

namespace Redcat.Xmpp.Negotiators
{
    public class SaslNegotiator : IFeatureNegatiator
    {
        private ConnectionSettings settings;

        public SaslNegotiator()
        {            
        }

        public bool CanNeogatiate(XmlElement feature)
        {
            return feature.Name == "mechanisms";
        }

        public bool Neogatiate(IXmppStream stream, XmlElement feature)
        {            
            throw new NotImplementedException();
        }

        private void PlainAuthenticator(IXmppStream stream, ConnectionSettings settings)
        {
            XmlElement auth = new XmlElement("auth", Namespaces.Sasl);
            auth.SetAttributeValue("mechanism", "PLAIN");
            auth.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes((char)0 + settings.Username + (char)0 + settings.Password));
            stream.Write(auth);
            var response = stream.Read();
        }
    }
}
