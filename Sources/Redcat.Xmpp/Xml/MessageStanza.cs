namespace Redcat.Xmpp.Xml
{
    public class MessageStanza : Stanza
    {
        public MessageStanza() : base("message")
        { }

        public string Subject
        {
            get { return GetAttributeValue<string>("subject"); }
            set { SetAttributeValue("subject", value); }
        }

        public string Body
        {
            get { return GetAttributeValue<string>("body"); }
            set { SetAttributeValue("body", value); }
        }
    }
}
