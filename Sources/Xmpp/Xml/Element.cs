﻿using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace Redcat.Xmpp.Xml
{
    public abstract class Element
    {
        private static XmlWriterSettings writerSettings;

        protected static XmlWriterSettings WriterSettings
        {
            get { return writerSettings; }
        }

        protected static XmlWriter CreateWriter(StringBuilder builder)
        {
            return XmlWriter.Create(builder, WriterSettings);
        }

        static Element()
        {
            writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
        }

        private Dictionary<string, object> attributes;        
        private string name;

        protected Element(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            this.name = name;
            attributes = new Dictionary<string, object>();
        }        

        public string Name
        {
            get { return name; }
        }

        #region Attribute methods

        protected bool HasAttribute(string name)
        {
            return attributes.ContainsKey(name);
        }       

        protected object GetAttributeValue(string name)
        {
            if (!attributes.ContainsKey(name)) return null;
            return attributes[name];
        }

        protected void SetAttributeValue(string name, object value)
        {
            attributes[name] = value;
        }

        #endregion

        #region Write methods

        public void Write(StringBuilder stringBuilder)
        {
            using(XmlWriter writer = CreateWriter(stringBuilder)) Write(writer);
        }

        public virtual void Write(XmlWriter writer)
        {
            WriteStartElement(writer);
            WriteAttributes(writer);
            WritePayload(writer);
            WriteEndElement(writer);
        }

        protected virtual void WriteStartElement(XmlWriter writer)
        {
            writer.WriteStartElement(name);
        }

        protected void WriteElementName(XmlWriter writer, string xmlns = null)
        {
            if (!string.IsNullOrEmpty(xmlns)) writer.WriteStartElement(name, xmlns);
            else writer.WriteStartElement(name);
        }

        protected void WriteElementName(XmlWriter writer, string prefix, string prefixUri, string xmlns = null)
        {
            writer.WriteStartElement(prefix, name, prefixUri);
            if (!string.IsNullOrEmpty(xmlns)) writer.WriteAttributeString("xmlns", xmlns);
        }

        protected virtual void WriteAttributes(XmlWriter writer)
        {
            if (attributes == null || attributes.Count == 0) return;
            foreach (string attrName in attributes.Keys)
            {
                if (attributes[attrName] == null) continue;
                if (attributes[attrName].ToString() == String.Empty) continue;
                writer.WriteAttributeString(attrName, attributes[attrName].ToString());
            }
        }

        protected virtual void WriteEndElement(XmlWriter writer)
        {
            writer.WriteEndElement();
        }

        protected virtual void WritePayload(XmlWriter writer)
        {
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Element)) return false;

            return Equals((Element) obj);
        }

        public bool Equals(Element element)
        {
            if (element == null) return false;
            return Name.Equals(element.Name) && AreAttributesEquals(this, element);
        }

        private bool AreAttributesEquals(Element element1, Element element2)
        {
            foreach (string attributeId in element1.attributes.Keys)
            {
                if (!element2.attributes.ContainsKey(attributeId)) return false;
                if (!AreValuesEquals(attributes[attributeId], element2.attributes[attributeId])) return false;
            }
            return true;
        }

        protected bool AreValuesEquals(object val1, object val2)
        {
            if (val1 == null && val2 == null) return true;
            if (val1 == null) return false;
            return val1.Equals(val2);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Write(sb);
            return sb.ToString();
        }
    }
}
