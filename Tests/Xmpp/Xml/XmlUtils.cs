using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    public static class XmlUtils
    {
        public static XElement CreateElement(string name, string xmlns = "", string prefix = "", string prefixUri = "")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            
            if (!string.IsNullOrEmpty(prefix))
            {
                XNamespace ns = prefixUri;
                XElement element = new XElement(ns + name, new XAttribute(XNamespace.Xmlns + prefix, prefixUri));
                
                if (!string.IsNullOrEmpty(xmlns)) element.Add(new XAttribute("xmlns", xmlns));
                return element;
            }

            return new XElement(XName.Get(name, xmlns));
        }

        public static void SetXmlLangValue(this XElement element, string value)
        {
            element.SetAttributeValue(XNamespace.Xml + "lang", value);
        }

        public static XElement CreateDatagramElement(string name, JID from, JID to, object id, string xmlLang)
        {
            XElement element = CreateDatagramElement(name, from, to, id);
            if (!string.IsNullOrEmpty(xmlLang)) element.SetXmlLangValue(xmlLang);
            return element;
        }

        public static XElement CreateDatagramElement(string name, JID from, JID to, object id)
        {
            return CreateDatagramElement(name, "", "", "", from, to, id);
        }

        public static XElement CreateDatagramElement(string name, string xmlns, string prefix, string prefixUri, JID from,
                                                     JID to, object id)
        {
            XElement datagramElement = CreateElement(name, xmlns, prefix, prefixUri);
            if (from != null) datagramElement.SetAttributeValue("from", from);
            if (to != null) datagramElement.SetAttributeValue("to", to);
            if (id != null) datagramElement.SetAttributeValue("id", id);
            return datagramElement;
        }

        public static XmlReader CreateXmlReader(XElement element)
        {
            return CreateXmlReader(element.ToString());
        }

        public static XmlReader CreateXmlReader(string xml)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            return XmlReader.Create(new StringReader(xml), settings);
        }

        public static XElement CreateStreamHeader(string xmlns, JID from = null, JID to = null,
                                                  object id = null)
        {
            XElement header = CreateDatagramElement("stream", xmlns, "stream", Namespaces.Streams, from, to, id);
            header.SetAttributeValue("version", "1.0");
            header.SetAttributeValue("xmlns", xmlns);
            return header;
        }

        public static XElement CreateStreamHeader(string streamNamespace, string version)
        {
            XElement header = CreateElement("stream", "stream", Namespaces.Streams);
            header.SetAttributeValue("version", version);
            header.SetAttributeValue("xmlns", streamNamespace);
            return header;
        }

        public static XElement CreatePresenceStanza(string type, string avaibility, string status, int? priority)
        {
            XElement element = CreateStanza("presence", type);
            if (!string.IsNullOrEmpty(avaibility)) element.Add(new XElement("show", avaibility));
            if (!string.IsNullOrEmpty(status)) element.Add(new XElement("status", status));
            if (priority != null) element.Add(new XElement("priority", priority));
            return element;
        }

        public static XElement CreateMessageStanza(string type, string bodyText = "", string subject = "",
                                                   string threadId = "", string parentThreadId = "")
        {
            XElement stanza = CreateStanza("message", type);
            if (!string.IsNullOrEmpty(bodyText)) stanza.Add(new XElement("body", bodyText));
            if (!string.IsNullOrEmpty(subject)) stanza.Add(new XElement("subject", subject));
            if (!string.IsNullOrEmpty(threadId))
            {
                XElement thread = new XElement("thread", threadId);
                if (!string.IsNullOrEmpty(parentThreadId)) thread.SetAttributeValue("parent", parentThreadId);
                stanza.Add(thread);
            }
            return stanza;
        }

        public static XElement CreateStanza(string name, string type, JID from = null, JID to = null, object id = null)
        {
            XElement stanza = CreateElement(name);
            stanza.SetAttributeValue("type", type);
            stanza.SetAttributeValue("from", from);
            stanza.SetAttributeValue("to", to);
            stanza.SetAttributeValue("id", id);
            return stanza;
        }
    }
}

