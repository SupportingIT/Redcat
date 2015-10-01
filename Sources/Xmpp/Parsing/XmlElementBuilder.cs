using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public class XmlElementBuilder : IXmlElementBuilder
    {
        public bool CanBuild(string name)
        {
            throw new System.NotImplementedException();
        }

        public void NewElement(string name)
        {
            throw new System.NotImplementedException();
        }

        public void AddAttribute(string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public void StartNode(string name)
        {
            throw new System.NotImplementedException();
        }

        public void SetNodeValue(string value)
        {
            throw new System.NotImplementedException();
        }

        public void EndNode()
        {
            throw new System.NotImplementedException();
        }

        public XmlElement Element { get; private set; }
    }
}
