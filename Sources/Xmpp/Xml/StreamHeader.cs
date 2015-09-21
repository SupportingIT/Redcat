using System.Xml;

namespace Redcat.Xmpp.Xml
{
    public class StreamHeader : DatagramElement
    {
        private string xmlns;

        public StreamHeader(string xmlns) : base("stream")
        {
            Version = "1.0";
            this.xmlns = xmlns;
        }

        public string Version
        {
            get { return (string) GetAttributeValue("version"); }
            set { SetAttributeValue("version", value); }
        }

        public bool WriteXmlDeclaration { get; set; }

        protected override void WriteStartElement(XmlWriter writer)
        {
            if (WriteXmlDeclaration) writer.WriteRaw("<?xml version='1.0'?>");
            WriteElementName(writer, "stream", Namespaces.Streams, xmlns);
        }

        protected override void WriteEndElement(XmlWriter writer)
        { }
    }
}
