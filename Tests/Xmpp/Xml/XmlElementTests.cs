using System.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class XmlElementTests
    {
        private readonly string name = "element-name";
        private readonly string xmlns = "element-namespace";

        [Test]
        public void Constructor_Correctly_Sets_Name()
        {
            XmlElement element = new XmlElement(name);

            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Xmlns, Is.Null);
            Assert.That(element.GetAttributeValue<string>("xmlns"), Is.Null);
        }

        [Test]
        public void Constructor_Correctly_Sets_Name_And_Xmlns()
        {
            XmlElement element = new XmlElement(name, xmlns);

            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Xmlns, Is.EqualTo(xmlns));
            Assert.That(element.GetAttributeValue<string>("xmlns"), Is.EqualTo(xmlns));
        }

        [Test]
        public void Constructor_Correctly_Sets_Name_Xmlns_Prefix_And_Prefix_Xmlns()
        {
            string prefix = "pref";
            string prefixNs = "prefNs";

            XmlElement element = new XmlElement(prefix, name, prefixNs, xmlns);

            Assert.That(element.Name, Is.EqualTo(prefix+":"+name));
            Assert.That(element.GetAttributeValue<string>("xmlns:"+prefix), Is.EqualTo(prefixNs));
        }

        [Test]
        public void Attributes_Returns_Elements_Attribute_Collection()
        {
            var expectedAttributes = Enumerable.Range(0, 3).Select(i => new XmlAttribute("attr" + i, i));
            XmlElement element = new XmlElement(name);

            foreach (var attribute in expectedAttributes) element.SetAttributeValue(attribute.Name, attribute.Value);
            
            Assert.That(element.Attributes, Is.EquivalentTo(expectedAttributes));
        }
    }
}
