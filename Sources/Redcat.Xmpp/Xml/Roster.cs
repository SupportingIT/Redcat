using System;

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
    }
}
