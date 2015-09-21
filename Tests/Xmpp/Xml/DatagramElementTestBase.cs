using System;
using System.Xml.Linq;

namespace Redcat.Xmpp.Tests.Xml
{
    public abstract class DatagramElementTestBase : ElementTestBase
    {
        protected readonly JID FromJid = "from@stanza_test.com";
        protected readonly JID ToJid = "to@stanza_test.com";
        protected readonly object Id = Guid.NewGuid();

        protected DatagramElementTestBase()
        { }

        protected DatagramElementTestBase(string name, string xmlns) : base(name, xmlns)
        { }

        protected XElement CreateDatagramXElement(JID from, JID to, object id)
        {
            return CreateDatagramXElement(Name, from, to, id);
        }

        protected XElement CreateDatagramXElement(JID from, JID to, object id, string xmlLang)
        {
            XElement element = CreateDatagramXElement(from, to, id);
            if (!string.IsNullOrEmpty(xmlLang)) element.SetAttributeValue(XNamespace.Xml + "lang", xmlLang);
            return element;
        }

        protected XElement CreateDatagramXElement(string name, JID from, JID to, object id)
        {
            return XmlUtils.CreateDatagramElement(name, from, to, id);
        }

        protected XElement CreateDatagramXElement(string name, string xmlns, string prefix, string prefixUri,  JID from, JID to, object id)
        {
            return XmlUtils.CreateDatagramElement(name, xmlns, prefix, prefixUri, from, to, id);
        }
    }
}
