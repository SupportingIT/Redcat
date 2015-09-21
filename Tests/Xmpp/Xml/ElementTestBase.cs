using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FakeItEasy;
using Microsoft.XmlDiffPatch;
using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    public abstract class ElementTestBase
    {
        private string name;
        private string xmlns;

        protected ElementTestBase() : this("test-element", "")
        { }
        
        protected ElementTestBase(string name, string xmlns)
        {
            this.name = name;
            this.xmlns = xmlns;
        }

        protected string Name
        {
            get { return name; }
        }

        protected string Xmlns
        {
            get { return xmlns; }
        }

        protected XElement CreateDefaultXElement()
        {
            return CreateXElement(name, xmlns);
        }

        protected XElement CreateXElement(string name, string xmlns = "", string prefix = "", string prefixUri = "")
        {
            return XmlUtils.CreateElement(name, xmlns, prefix, prefixUri);
        }

        protected T CreateStreamElementMock<T>(params object[] constructorParams) where T : Element
        {
            Fake<T> fake = new Fake<T>(o => o.WithArgumentsForConstructor(constructorParams));
            fake.AnyCall().CallsBaseMethod();
            return fake.FakedObject;
        }

        protected void VerifyWriteOutput(XElement expectedXml, Element element)
        {
            //XmlUtils.CreateXmlReader uses instead XElement.CreateReader
            //because last one don't treat 'xmlns' as attribute
            XmlReader expectedXmlReader = XmlUtils.CreateXmlReader(expectedXml);
            VerifyWriteOutput(expectedXmlReader, element);
        }

        protected void VerifyWriteOutput(string expectedXml, Element element)
        {
            XmlReader expectedXmlReader = XmlUtils.CreateXmlReader(expectedXml);
            VerifyWriteOutput(expectedXmlReader, element);
        }

        protected void VerifyWriteOutput(XmlReader expectedXml, Element element)
        {
            XmlReader acrtalXmlReader = CreateReaderForElement(element);
            XmlDiff diff = CreateXmlDiff();

            bool valid = diff.Compare(expectedXml, acrtalXmlReader);

            if (!valid) Assert.Fail("Expected {0} but was {1}", expectedXml, element);
        }

        private XmlReader CreateReaderForElement(Element element)
        {
            StringBuilder xml = new StringBuilder();
            XmlWriterSettings writerSettings = new XmlWriterSettings { OmitXmlDeclaration = true };
            XmlWriter writer = XmlWriter.Create(xml, writerSettings);
            
            element.Write(writer);
            writer.Close();

            return XmlReader.Create(new StringReader(xml.ToString()));
        }

        private XmlDiff CreateXmlDiff()
        {
            XmlDiff diff = new XmlDiff();
            diff.Algorithm = XmlDiffAlgorithm.Precise;
            diff.IgnoreWhitespace = true;
            diff.IgnoreChildOrder = true;
            return diff;
        }
    }
}
