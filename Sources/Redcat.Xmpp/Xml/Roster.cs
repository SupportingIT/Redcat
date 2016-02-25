namespace Redcat.Xmpp.Xml
{
    public static class Roster
    {
        public static IqStanza Get(JID from)
        {
            IqStanza iq = Iq.Get();
            iq.AddChild(new XmlElement("query", Namespaces.Roster));
            iq.From = from;
            iq.Id = "some-id";
            return iq;
        }
    }
}
