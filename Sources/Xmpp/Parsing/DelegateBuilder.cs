using System;
using System.Linq;
using System.Collections.Generic;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public class BuilderParser<T> : IXmlElementBuilder where T : XmlElement
    {
        private IDictionary<string, Action<BuildingContext>> attributeBuilders;
        private IDictionary<string, Action<BuildingContext>> nodeBuilders;
        private Func<string,T> createElement;
        private string[] supportedElements;
        private BuildingContext context;

        public BuilderParser(Func<string,T> createElementFunc, params string[] supportedElements)
        {
            attributeBuilders = new Dictionary<string, Action<BuildingContext>>();
            createElement = createElementFunc;
            context = new BuildingContext();
            this.supportedElements = supportedElements;
        }

        public IDictionary<string, Action<BuildingContext>> AttributeBuilders
        {
            get { return attributeBuilders; } }

        public XmlElement Element
        {
            get { return context.Element; }
        }

        public void AddAttribute(string name, string value)
        {
            if (context.Element == null) throw new InvalidOperationException();
            context.AttributeName = name;
            context.AttributeValue = value;
            if (attributeBuilders.ContainsKey(name)) attributeBuilders[name](context);
            context.AttributeName = context.AttributeValue = null;
        }

        public void AddNodeBuilder(string nodeName, Action<BuildingContext> valueBuilder)
        {
            if (nodeBuilders == null) nodeBuilders = new Dictionary<string, Action<BuildingContext>>();
            nodeBuilders.Add(nodeName, valueBuilder);
        }

        public bool CanBuild(string elementName)
        {
            return supportedElements.Any(s => string.CompareOrdinal(s, elementName) == 0);
        }

        public void EndNode()
        {
            context.NodeName = context.NodeValue = null;
        }

        public void NewElement(string name)
        {
            context.Element = createElement(name);
        }

        public void SetNodeValue(string value)
        {
            if (string.IsNullOrEmpty(context.NodeName)) throw new InvalidOperationException();
            if (nodeBuilders == null || !nodeBuilders.ContainsKey(context.NodeName)) return;
            context.NodeValue = value;
            nodeBuilders[context.NodeName](context);
        }

        public void StartNode(string name)
        {
            context.NodeName = name;
        }

        public class BuildingContext
        {
            internal BuildingContext() { }

            public T Element { get; internal set; }

            public string AttributeName { get; internal set; }
            public string AttributeValue { get; internal set; }
            
            public string NodeName { get; internal set; }
            public string NodeValue { get; internal set; }

            public int Depth { get; internal set; }
        }
    }    
}
