using System.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System.Collections.Generic;
using System;

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

        [Test]
        public void ForEachAttribute_Iterates_Over_Each_Attribute()
        {
            Dictionary<string, object> expectedAttributes = new Dictionary<string, object>{ { "value1", 1}, { "value2", "value" } };
            XmlElement element = new XmlElement("element");
            foreach (var attribute in expectedAttributes) element.SetAttributeValue(attribute.Key, attribute.Value);
            Dictionary<string, object> actualAttributes = new Dictionary<string, object>();

            element.ForEachAttribute((n, v) => actualAttributes.Add(n, v));

            Assert.That(actualAttributes, Is.EquivalentTo(expectedAttributes));
        }

        [Test]
        public void GetAttributeValue_Returns_Default_Value_If_Existed_Value_Type_Differend()
        {
            XmlElement element = new XmlElement("element");
            element.SetAttributeValue("attr", "value");

            Assert.That(element.GetAttributeValue<int>("attr"), Is.EqualTo(0));
            Assert.That(element.GetAttributeValue<Delegate>("attr"), Is.EqualTo(null));
        }

        [Test]
        public void GetAttributeValue_Returns_Correct_Attribute_Value()
        {
            XmlElement element = new XmlElement("element");
            Guid value = Guid.NewGuid();
            element.SetAttributeValue("attr", value);

            Assert.That(element.GetAttributeValue<Guid>("attr"), Is.EqualTo(value));
        }
    }
}
