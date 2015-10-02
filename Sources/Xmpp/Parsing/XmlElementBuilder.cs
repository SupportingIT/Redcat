using System.Collections.Generic;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public class XmlElementBuilder : XmlElementBuilderBase
    {
        private readonly Stack<XmlElement> nodesBranch = new Stack<XmlElement>();

        protected XmlElement CurrentNode
        {
            get { return nodesBranch.Peek(); }
        }

        public override bool CanBuild(string name)
        {
            return true;
        }

        protected override XmlElement CreateElement(string elementName)
        {
            XmlElement element = new XmlElement(elementName);
            nodesBranch.Push(element);
            return element;
        }

        protected override void OnAddAttribute(BuilderContext context)
        {
            CurrentNode.SetAttributeValue(context.AttributeName, context.AttributeValue);
        }

        protected override void OnStartNode(BuilderContext context)
        {
            XmlElement element = new XmlElement(context.NodeName);
            CurrentNode.Childs.Add(element);
            nodesBranch.Push(element);
        }

        protected override void OnSetNodeValue(BuilderContext context)
        {
            CurrentNode.Value = context.NodeValue;
        }

        protected override void OnEndNode(BuilderContext context)
        {
            nodesBranch.Pop();
        }
    }
}
