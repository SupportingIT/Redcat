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

        public static IqStanza AddItem(Contact contact)
        {
            IqStanza iq = Iq.Set();
            iq.AddQueryWithItems(false, contact);
            return iq;
        }

        public static IqStanza RemoveItem(Contact contact)
        {
            IqStanza iq = Iq.Set();
            iq.AddQueryWithItems(true, contact);
            return iq;
        }

        public static IqStanza Result(params Contact[] contacts)
        {
            IqStanza iq = Iq.Result();
            iq.AddQueryWithItems(false, contacts);
            return iq;
        }

        private static void AddQueryWithItems(this IqStanza iq, bool isRemove, params Contact[] contacts)
        {            
            var query = iq.AddQuery();
            foreach (var contact in contacts)
            {
                var item = new XmlElement("item");
                if (contact.Id is JID) item.SetAttributeValue("jid", (JID)contact.Id);
                item.SetAttributeValue("name", contact.Name);
                if (isRemove) item.SetAttributeValue("subscription", "remove");
                query.AddChild(item);
            }
        }
    }
}
