using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public class StanzaBuilder : XmlElementBuilder
    {
        protected override XmlElement CreateElementInstance(string elementName)
        {
            if (elementName == "iq") return new IqStanza();
            if (elementName == "presence") return new PresenceStanza();
            if (elementName == "message") return new MessageStanza();
            return base.CreateElementInstance(elementName);
        }

        protected override void OnAddAttribute(BuilderContext context)
        {
            if (context.AttributeName == "from" && CurrentNode is Stanza && JID.IsValid(context.AttributeValue))
            {
                ((Stanza)CurrentNode).From = JID.Parse(context.AttributeValue);
                return;
            }

            if (context.AttributeName == "to" && CurrentNode is Stanza && JID.IsValid(context.AttributeValue))
            {
                ((Stanza)CurrentNode).To = JID.Parse(context.AttributeValue);
                return;
            }

            base.OnAddAttribute(context);
        }
    }
}
