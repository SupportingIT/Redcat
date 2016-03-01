using Redcat.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp.Xml
{
    public static class Roster
    {
        public static IqStanza Request(object id = null, JID from = null)
        {
            IqStanza iq = Iq.Get();
            if (id != null) iq.Id = id;
            if (from != null) iq.From = from;
            iq.AddQuery();            
            return iq;
        }

        private static XmlElement AddQuery(this XmlElement element)
        {
            var query = Query();
            element.AddChild(query);
            return query;
        }

        private static XmlElement Query() => new IqQuery(Namespaces.Roster);

        public static bool IsRosterRequest(this IqStanza iq)
        {
            return iq.IsGet() && iq.HasChild("query", Namespaces.Roster);
        }

        public static bool IsRosterResponse(this IqStanza iq)
        {
            return iq.IsResult() && iq.HasChild("query");
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
            stanza.AddQuery().AddChild(item);
            return stanza;
        }

        public static IqStanza RemoveItem(JID jid)
        {
            IqStanza stanza = Iq.Set();
            XmlElement item = new XmlElement("item");
            item.SetAttributeValue("jid", jid);
            item.SetAttributeValue("subscription", "remove");            
            stanza.AddQuery().AddChild(item);
            return stanza;
        }
    }
}
