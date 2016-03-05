using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp.Xml
{
    public static class Roster
    {
        public static bool IsRosterIq(this IqStanza iq)
        {
            return iq.HasChild("query");
        }

        public static IqStanza Request(JID from = null)
        {
            IqStanza iq = Iq.Get();
            if (from != null) iq.From = from;
            iq.AddRosterQuery();            
            return iq;
        }

        public static XmlElement AddRosterQuery(this XmlElement element)
        {
            var query = Query();
            element.AddChild(query);
            return query;
        }

        private static XmlElement Query() => new IqQuery(Namespaces.Roster);

        public static bool IsRosterRequest(this IqStanza iq)
        {
            return iq.IsGet() && iq.IsRosterIq();
        }

        public static bool IsRosterResponse(this IqStanza iq)
        {
            return iq.IsResult() && iq.IsRosterIq();
        }

        public static IEnumerable<XmlElement> GetRosterItems(this IqStanza iq)
        {
            var query = iq.Child("query");
            return query.Childs.Where(c => c.Name == "item");
        }

        public static bool IsRosterPush(this IqStanza stanza)
        {
            return stanza.IsSet() && stanza.GetRosterItems().Any();
        }

        public static IqStanza AddItem(JID jid, string name = null)
        {
            IqStanza stanza = Iq.Set();
            XmlElement item = new XmlElement("item");
            item.SetAttributeValue("jid", jid);
            if (name != null) item.SetAttributeValue("name", name);
            stanza.AddRosterQuery().AddChild(item);
            return stanza;
        }

        public static IqStanza RemoveItem(JID jid)
        {
            IqStanza stanza = Iq.Set();
            XmlElement item = new XmlElement("item");
            item.SetAttributeValue("jid", jid);
            item.SetAttributeValue("subscription", "remove");            
            stanza.AddRosterQuery().AddChild(item);
            return stanza;
        }
    }
}
