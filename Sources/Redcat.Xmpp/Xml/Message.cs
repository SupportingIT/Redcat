namespace Redcat.Xmpp.Xml
{
    public static class Message
    {
        public static MessageStanza SetBody(this MessageStanza message, string text)
        {
            message.SetAttributeValue("body", text);
            return message;
        }

        public static MessageStanza CreateMessage(JID to, string type, string message)
        {
            return new MessageStanza { To = to, Type = type }.SetBody(message);
        }

        public static MessageStanza Chat(JID to, string message)
        {
            return CreateMessage(to, "chat", message);
        }
    }
}
