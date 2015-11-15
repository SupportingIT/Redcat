using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Text;

namespace Redcat.Xmpp.Negotiators
{
    public static class Authenticators
    {
        public static void Plain(IXmppStream stream, ConnectionSettings settings)
        {
            XmlElement auth = new XmlElement("auth", Namespaces.Sasl);
            auth.SetAttributeValue("mechanism", "PLAIN");
            auth.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes((char)0 + settings.Username + (char)0 + settings.Password));
            stream.Write(auth);
            var response = stream.Read();
        }
    }
}
