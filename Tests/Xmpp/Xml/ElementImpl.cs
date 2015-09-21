using System.Xml;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    internal class ElementImpl : Element
    {
        public ElementImpl(string name) : base(name)
        { }

        public string Content { get; set; }

        public object this[string name]
        {
            get { return GetAttributeValue(name); }
            set { SetAttributeValue(name, value); }
        }

        protected override void WritePayload(XmlWriter writer)
        {
            if (!string.IsNullOrEmpty(Content)) writer.WriteString(Content);
        }
    }
}
