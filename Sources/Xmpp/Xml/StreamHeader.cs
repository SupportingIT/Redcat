using System.Xml;

namespace Redcat.Xmpp.Xml
{
    public class StreamHeader : DatagramElement
    {
        public StreamHeader(string xmlns = null) : base("stream")
        {
            Version = "1.0";
            Xmlns = xmlns;
        }

        public string Xmlns { get; set; }

        public string Version
        {
            get { return (string) GetAttributeValue("version"); }
            set { SetAttributeValue("version", value); }
        }

        public bool WriteXmlDeclaration { get; set; }

        protected override void WriteStartElement(XmlWriter writer)
        {
            if (WriteXmlDeclaration) writer.WriteRaw("<?xml version='1.0'?>");
            WriteElementName(writer, "stream", Namespaces.Streams, Xmlns);
        }

        protected override void WriteEndElement(XmlWriter writer)
        { }
    }
}
