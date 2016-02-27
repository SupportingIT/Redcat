using System;
using Redcat.Xmpp.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class XmppStreamWriter : IDisposable
    {
        private static Encoding defaultEncoding = Encoding.UTF8;
        private TextWriter writer;

        public XmppStreamWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            writer = new StreamWriter(stream, defaultEncoding);            
        }

        public XmppStreamWriter(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            this.writer = writer;
        }

        public void Write(string xml)
        {
            writer.Write(xml);
            writer.Flush();
        }

        public void Write(XmlElement element)
        {
            string closingChars = " />";
            if (IsStreamHeader(element))
            {
                WriteDeclaration();
                closingChars = ">";
            }

            WriteElementStart(element.Name);
            WriteAttributes(element.Attributes);
            
            if (element.HasContent) WriteContent(element);
            else writer.Write(closingChars);
            writer.Flush();
        }

        private bool IsStreamHeader(XmlElement element)
        {
            return element.Name == "stream:stream";
        }

        private void WriteDeclaration()
        {
            writer.Write("<?xml version='1.0' ?>");
        }

        private void WriteElementStart(string name)
        {
            writer.Write('<');
            writer.Write(name);
        }

        private void WriteAttributes(IEnumerable<XmlAttribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                writer.Write(" ");
                writer.Write(attribute.Name);
                writer.Write("='");
                writer.Write(attribute.Value);
                writer.Write("'");
            }
        }

        private void WriteContent(XmlElement element)
        {
            writer.Write(">");
            writer.Write(element.Value);
            WriteChilds(element);
            WriteClosingTag(element.Name);
        }

        private void WriteChilds(XmlElement element)
        {
            if (element.HasChilds)
            {
                foreach (var child in element.Childs) Write(child);
            }
        }

        private void WriteClosingTag(string name)
        {
            writer.Write("</");
            writer.Write(name);
            writer.Write(">");
        }

        public void Dispose() => writer.Dispose();
    }
}
