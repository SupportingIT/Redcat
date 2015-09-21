using System;
using System.Linq;
using System.Collections.Generic;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public class DelegateParser<T> : IElementParser where T : Element
    {
        private IDictionary<string, Action<ParsingContext>> attributeBuilders;
        private IDictionary<string, Action<ParsingContext>> nodeBuilders;
        private Func<string,T> createElement;
        private string[] supportedElements;
        private ParsingContext context;

        public DelegateParser(Func<string,T> createElementFunc, params string[] supportedElements)
        {
            attributeBuilders = new Dictionary<string, Action<ParsingContext>>();
            createElement = createElementFunc;
            context = new ParsingContext();
            this.supportedElements = supportedElements;
        }

        public IDictionary<string, Action<ParsingContext>> AttributeBuilders
        {
            get { return attributeBuilders; } }

        public Element ParsedElement
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

        public void AddNodeBuilder(string nodeName, Action<ParsingContext> valueBuilder)
        {
            if (nodeBuilders == null) nodeBuilders = new Dictionary<string, Action<ParsingContext>>();
            nodeBuilders.Add(nodeName, valueBuilder);
        }

        public bool CanParse(string elementName)
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

        public class ParsingContext
        {
            internal ParsingContext() { }

            public T Element { get; internal set; }

            public string AttributeName { get; internal set; }
            public string AttributeValue { get; internal set; }
            
            public string NodeName { get; internal set; }
            public string NodeValue { get; internal set; }

            public int Depth { get; internal set; }
        }
    }    
}
