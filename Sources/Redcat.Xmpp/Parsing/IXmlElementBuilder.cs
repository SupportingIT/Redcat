using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public interface IXmlElementBuilder
    {
        bool CanBuild(string name);
        void NewElement(string name);
        void AddAttribute(string name, string value);
        void StartNode(string name);
        void SetNodeValue(string value);
        void EndNode();

        XmlElement Element { get; }
    }
}
