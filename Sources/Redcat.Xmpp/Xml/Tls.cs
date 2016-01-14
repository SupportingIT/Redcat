namespace Redcat.Xmpp.Xml
{
    public static class Tls
    {
        public static readonly XmlElement Start = new XmlElement("starttls", Namespaces.Tls);
        public static readonly XmlElement Proceed = new XmlElement("proceed", Namespaces.Tls);
        public static readonly XmlElement Failure = new XmlElement("failure", Namespaces.Tls);
    }
}
