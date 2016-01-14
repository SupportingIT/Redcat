using System;
using System.Collections.Generic;
using System.Linq;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public abstract class XmlElementBuilderBase : IXmlElementBuilder
    {
        private BuilderContext context;
        private string[] supportedElements;

        protected XmlElementBuilderBase(params string[] supportedElements)
        {
            context = new BuilderContext();
            this.supportedElements = supportedElements;
        }

        public virtual bool CanBuild(string name)
        {
            return supportedElements.Any(s => string.CompareOrdinal(s, name) == 0);
        }

        public void NewElement(string name)
        {
            if (!CanBuild(name)) throw new InvalidOperationException();
            context.RootElement = CreateElement(name);
            context.NodeNames.Push(name);
        }

        protected abstract XmlElement CreateElement(string elementName);

        public void AddAttribute(string name, string value)
        {
            context.AttributeName = name;
            context.AttributeValue = value;
            OnAddAttribute(context);
        }

        protected abstract void OnAddAttribute(BuilderContext context);

        public void StartNode(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            context.NodeNames.Push(name);
            context.Depth++;
            OnStartNode(context);
        }

        protected abstract void OnStartNode(BuilderContext context);

        public void SetNodeValue(string value)
        {
            context.NodeValue = value;
            OnSetNodeValue(context);
        }

        protected abstract void OnSetNodeValue(BuilderContext context);

        public void EndNode()
        {            
            OnEndNode(context);
            context.Depth--;
            context.NodeNames.Pop();            
        }

        protected abstract void OnEndNode(BuilderContext context);

        public XmlElement Element
        {
            get { return context.RootElement; }
        }
    }

    public class BuilderContext
    {
        private Stack<string> nodes;

        internal BuilderContext()
        {
            nodes = new Stack<string>();
        }

        public string AttributeName { get; internal set; }
        public string AttributeValue { get; internal set; }
        public int Depth { get; internal set; }
        public string NodeName { get { return nodes.Peek(); } }
        public string NodeValue { get; internal set; }

        internal Stack<string> NodeNames
        {
            get { return nodes; }
        }

        public XmlElement RootElement { get; internal set; }
    }
}
