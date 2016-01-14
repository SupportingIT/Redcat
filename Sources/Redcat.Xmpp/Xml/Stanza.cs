namespace Redcat.Xmpp.Xml
{
    public class Stanza : DatagramElement
    {
        public Stanza(string name) : base(name)
        { }

        public Stanza(string name, string type) : base(name)
        {
            Type = type;
        }

        public string Type
        {
            get { return GetAttributeValue<string>("type"); }
            set { SetAttributeValue("type", value); }
        }
    }
}
