﻿using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Redcat.Xmpp
{
    public class XmppStreamReader
    {
        private const int BufferSize = 10000;
        private static readonly Encoding defaultEncoding = Encoding.UTF8;

        private Queue<XmlElement> elementQueue = new Queue<XmlElement>();
        private char[] buffer = new char[BufferSize];
        private IXmlParser parser;
        private TextReader reader;        

        public XmppStreamReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));            
            reader = new StreamReader(stream, defaultEncoding);
        }

        public XmppStreamReader(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            this.reader = reader;
        }

        public IXmlParser Parser
        {
            get { return parser ?? (parser = CreateDefaultParser()); }
            set { parser = value; }
        }

        private IXmlParser CreateDefaultParser()
        {
            return new XmppStreamParser();
        }

        public XmlElement Read()
        {
            if (elementQueue.Count > 0) return elementQueue.Dequeue();            
            int readed = reader.Read(buffer, 0, buffer.Length);
            string xml = new string(buffer, 0, readed);
            foreach (var element in Parser.Parse(xml))
            {
                elementQueue.Enqueue(element);
            }

            if (elementQueue.Count == 0) return null;
            return elementQueue.Dequeue();
        }
    }
}
