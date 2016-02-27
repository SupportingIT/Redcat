using System;

namespace Redcat.Xmpp.Xml
{
    public static class Roster
    {
        public static IqStanza Get(JID from)
        {
            IqStanza iq = Iq.Get();
            iq.Id = Guid.NewGuid();
            iq.AddChild(new XmlElement("query", Namespaces.Roster));
            iq.From = from;
            return iq;
        }
    }
}
