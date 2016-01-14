namespace Redcat.Xmpp.Xml
{
    public class IqStanza : Stanza
    {
        public IqStanza() : base("iq")
        { }

        public IqStanza(string type) : this()
        {
            Type = type;
        }
    }
}
