using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public interface IElementParser
    {
        bool CanParse(string elementName);

        Element ParsedElement { get; }

        void AddAttribute(string name, string value);

        void NewElement(string name);

        void StartNode(string name);

        void SetNodeValue(string value);

        void EndNode();
    }
}
