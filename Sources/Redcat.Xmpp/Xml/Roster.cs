using System;

namespace Redcat.Xmpp.Xml
{
    public static class Roster
    {
        public static IqStanza Get(JID from)
        {
            IqStanza iq = Iq.Get();
            iq.Id = Guid.NewGuid();
            iq.AddChild(Query());
            iq.From = from;
            return iq;
        }

        public static IqStanza Set(JID itemJid)
        {
            IqStanza iq = Iq.Set();
            iq.Id = "123";
            var item = new XmlElement("item");
            item.SetAttributeValue("jid", itemJid);
            iq.AddQuery().AddChild(item);
            return iq;
        }

        private static XmlElement AddQuery(this XmlElement element)
        {
            var query = Query();
            element.AddChild(query);
            return query;
        }

        private static XmlElement Query() => new XmlElement("query", Namespaces.Roster);
    }
}
