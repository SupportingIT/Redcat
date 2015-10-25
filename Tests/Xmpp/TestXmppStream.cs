using Redcat.Xmpp.Xml;
using System.Collections.Generic;

namespace Redcat.Xmpp.Tests
{
    internal class TestXmppStream : IXmppStream
    {
        private Queue<XmlElement> sendedElements = new Queue<XmlElement>();
        private Queue<XmlElement> receivedElements = new Queue<XmlElement>();

        public Queue<XmlElement> ReceivedElements
        {
            get { return receivedElements; }
        }

        public Queue<XmlElement> SendedElements
        {
            get { return sendedElements; }
        }

        public void EnqueueResponse(XmlElement response)
        {
            receivedElements.Enqueue(response);
        }

        public XmlElement GetSentElement()
        {
            return sendedElements.Dequeue();
        }

        public XmlElement Read()
        {
            return receivedElements.Dequeue();
        }

        public void Write(XmlElement element)
        {
            sendedElements.Enqueue(element);
        }
    }
}
