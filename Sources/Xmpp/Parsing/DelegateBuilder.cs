using System;
using System.Collections.Generic;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public abstract class DelegateBuilder<T> : XmlElementBuilderBase where T : XmlElement
    {
        private IDictionary<string, Action<T, string>> attributeSetters;
        private IDictionary<string, Action<T, string>> valueSetters;
        private T currentElement;

        protected DelegateBuilder(params string[] supportedElements) : base(supportedElements)
        {
            attributeSetters = new Dictionary<string, Action<T, string>>();
            valueSetters = new Dictionary<string, Action<T, string>>();
        }

        protected T CurrentElement
        {
            get { return currentElement; }
        }

        protected override XmlElement CreateElement(string elementName)
        {
            currentElement = CreateInstance(elementName);
            return currentElement;
        }

        protected abstract T CreateInstance(string elementName);

        protected override void OnAddAttribute(BuilderContext context)
        {
            if (!attributeSetters.ContainsKey(context.AttributeName))
                Element.SetAttributeValue(context.AttributeName, context.AttributeValue);
            else
                attributeSetters[context.AttributeName](currentElement, context.AttributeValue);
        }

        protected void Attribute(string name, Action<T, string> setter)
        {
            attributeSetters[name] = setter;
        }

        protected void NodeValue(string nodeName, Action<T, string> setter)
        {
            valueSetters[nodeName] = setter;
        }

        protected override void OnStartNode(BuilderContext context)
        { }

        protected override void OnSetNodeValue(BuilderContext context)
        {
            if (valueSetters.ContainsKey(context.NodeName))
                valueSetters[context.NodeName](CurrentElement, context.NodeValue);
        }

        protected override void OnEndNode(BuilderContext context)
        { }        
    }
}
