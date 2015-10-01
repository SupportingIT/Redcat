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

        public bool CanBuild(string name)
        {
            return supportedElements.Any(s => string.CompareOrdinal(s, name) == 0);
        }

        public void NewElement(string name)
        {
            if (!CanBuild(name)) throw new InvalidOperationException();
            context.RootElement = CreateElement(name);
        }

        protected abstract XmlElement CreateElement(string elementName);

        public void AddAttribute(string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public void StartNode(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            context.NodeName = name;
            context.Depth++;
            OnStartNode(context);
        }

        protected abstract void OnStartNode(BuilderContext context);

        public void SetNodeValue(string value)
        {
            throw new System.NotImplementedException();
        }

        public void EndNode()
        {
            throw new System.NotImplementedException();
        }

        public XmlElement Element
        {
            get { return context.RootElement; }
        }
    }

    public class BuilderContext
    {
        internal BuilderContext()
        {
        }

        public string AttributeName { get; internal set; }
        public string AttributeValue { get; internal set; }
        public int Depth { get; internal set; }
        public string NodeName { get; internal set; }
        public string NodeValue { get; internal set; }

        public XmlElement RootElement { get; internal set; }
    }
}
