namespace Redcat.Xmpp.Xml
{
    public class DatagramElement : XmlElement
    {
        public DatagramElement(string name, string xmlns = null) : base(name, xmlns)
        {
        }

        public object Id 
        {
            get { return GetAttributeValue<object>("id"); }
            set { SetAttributeValue("id", value); } 
        }

        public JID From
        {
            get { return GetAttributeValue<JID>("from"); }
            set { SetAttributeValue("from", value); }
        }

        public JID To
        {
            get { return GetAttributeValue<JID>("to"); }
            set { SetAttributeValue("to", value); }
        }

        public string XmlLang
        {
            get { return GetAttributeValue<string>("xml:lang"); }
            set { SetAttributeValue("xml:lang", value); }
        }
    }
}
