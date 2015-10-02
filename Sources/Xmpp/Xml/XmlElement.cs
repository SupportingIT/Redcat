using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Redcat.Xmpp.Xml
{
    public class XmlElement
    {
        private string name;
        private IDictionary<string, object> attributes;
        private ICollection<XmlElement> childs;

        public XmlElement(string name, string xmlns = null)
        {
            attributes = new Dictionary<string, object>();
            this.name = name;
            if (!string.IsNullOrEmpty(xmlns)) SetAttributeValue("xmlns", xmlns);
        }

        public XmlElement(string prefix, string name, string prefixXmlns, string xmlns = null) : this(string.Format("{0}:{1}", prefix, name), xmlns)
        {
            SetAttributeValue("xmlns:"+prefix, prefixXmlns);
        }

        public IEnumerable<XmlAttribute> Attributes
        {
            get { return attributes.Select(kvp => new XmlAttribute(kvp.Key, kvp.Value));}
        }

        public string Name
        { 
            get { return name; } 
        }

        public string Xmlns
        {
            get { return GetAttributeValue<string>("xmlns"); }
        }

        public object Value { get; set; }

        public ICollection<XmlElement> Childs
        {
            get { return childs ?? (childs = CreateChildsCollection()); }
        }

        public bool HasChilds
        {
            get { return childs != null && childs.Count > 0; }
        }

        public bool HasContent
        {
            get { return HasChilds || Value != null; }
        }

        private ICollection<XmlElement> CreateChildsCollection()
        {
            return new List<XmlElement>();
        }

        public T GetAttributeValue<T>(string name)
        {
            if (!attributes.ContainsKey(name) || !(attributes[name] is T)) return default(T);
            return (T)attributes[name];
        }

        public void SetAttributeValue<T>(string name, T value)
        {
            attributes[name] = value;
        }

        public void ForEachAttribute(Action<string, object> attributeAction)
        {
            foreach (var attribute in attributes) attributeAction(attribute.Key, attribute.Value);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            XmppStreamWriter xmppWriter = new XmppStreamWriter(writer);
            xmppWriter.Write(this);
            return builder.ToString();
        }
    }

    public struct XmlAttribute
    {
        public readonly string Name;
        public readonly object Value;

        public XmlAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
