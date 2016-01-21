using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Text;

namespace Redcat.Xmpp.Negotiators
{
    public static class Authenticators
    {
        public static XmlElement Plain(IXmppStream stream, ISaslCredentials credentials)
        {
            XmlElement auth = new XmlElement("auth", Namespaces.Sasl);
            auth.SetAttributeValue("mechanism", "PLAIN");
            auth.Value = GetAuthenticationString(credentials.Username, credentials.Password);
            stream.Write(auth);
            return stream.Read();
        }

        private static string GetAuthenticationString(string username, string password)
        {
            StringBuilder builder = new StringBuilder(username.Length + password.Length + 2);
            builder.Append('\u0000').Append(username)
                   .Append('\u0000').Append(password);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString()));
        }
    }
}
