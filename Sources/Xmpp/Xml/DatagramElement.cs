using System.Xml;

namespace Redcat.Xmpp.Xml
{
    public abstract class DatagramElement : CompositeElement
    {
        protected DatagramElement(string name) : base(name)
        {
        }

        public object Id 
        {
            get { return GetAttributeValue("id"); }
            set { SetAttributeValue("id", value); } 
        }

        public JID From
        {
            get { return (JID) GetAttributeValue("from"); }
            set { SetAttributeValue("from", value); }
        }

        public JID To
        {
            get { return (JID) GetAttributeValue("to"); }
            set { SetAttributeValue("to", value); }
        }

        public string XmlLang { get; set; }

        protected override void WriteAttributes(XmlWriter writer)
        {
            base.WriteAttributes(writer);
            if (!string.IsNullOrEmpty(XmlLang)) writer.WriteAttributeString("xml", "lang", null, XmlLang);
        }

        public override bool Equals(object obj)
        {
            DatagramElement element = obj as DatagramElement;
            return Equals(element);
        }

        public bool Equals(DatagramElement element)
        {
            return base.Equals(element) && AreValuesEquals(XmlLang, element.XmlLang);
        }
    }
}
