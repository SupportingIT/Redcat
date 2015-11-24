namespace Redcat.Xmpp.Xml
{
    public static class Bind
    {
        public static XmlElement New()
        {
            return new XmlElement("bind", Namespaces.Bind);
        }

        public static XmlElement Jid(JID jid)
        {
            var bind = New();
            bind.Childs.Add(new XmlElement("jid") { Value = jid });
            return bind;
        }

        public static XmlElement Resource(string resource)
        {
            var bind = New();
            bind.Childs.Add(new XmlElement("resource") { Value = resource });
            return bind;
        }
    }
}
