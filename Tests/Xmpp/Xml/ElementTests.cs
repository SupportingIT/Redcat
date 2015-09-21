using System;
using System.Xml.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    public class ElementTests : ElementTestBase
    {
        private readonly string attributeName = "amy";
        private readonly string attributeValue = "lee";

        #region Write method tests

        [Test]
        public void Write_CorrectXmlName_ProducesCorrectXmlElement()
        {
            Element actualElement = new ElementImpl(Name);
            XElement expectedElement = CreateXElement(Name);

            VerifyWriteOutput(expectedElement, actualElement);
        }
        
        [Test]
        public void Write_NonEmptyContent_ProduceCorrectXmlElementWithContent()
        {
            string payload = Guid.NewGuid().ToString();
            ElementImpl xmppElement = new ElementImpl(Name);
            XElement expectedElement = CreateXElementWithPayload(Name, payload);

            xmppElement.Content = payload;

            VerifyWriteOutput(expectedElement, xmppElement);
        }

        private XElement CreateXElementWithPayload(string name, string content)
        {
            XElement xmppElement = CreateXElement(name);
            xmppElement.SetValue(content);
            return xmppElement;
        }

        [Test]
        public void Write_EmptyAttributeValue_WillNotWriteAttribute()
        {
            ElementImpl xmppElement = new ElementImpl(Name);
            XElement expectedElement = CreateXElement(Name);
            xmppElement["nullAttribute"] = null;
            xmppElement["emptyAttribute"] = "";

            VerifyWriteOutput(expectedElement, xmppElement);
        }

        #endregion

        #region Attribute method tests

        [Test]
        public void GetAttributeValue_NotEmptyNameValue_ReturnsLastSettedValue()
        {
            ElementImpl xmppElement = new ElementImpl(Name);
            xmppElement[attributeName] = attributeValue;

            Assert.AreEqual(attributeValue, xmppElement[attributeName]);
        }

        [Test]
        public void SetAttributeValue_NonEmptyValue_WritesAttributeWithGivenValue()
        {
            ElementImpl xmppElement = new ElementImpl(Name);
            XElement expectedElement = CreateXElement(Name);
            expectedElement.SetAttributeValue(attributeName, attributeValue);
            xmppElement[attributeName] = attributeValue;

            VerifyWriteOutput(expectedElement, xmppElement);
        }

        #endregion
        
        #region Equals method test

        [Test]
        public void Equals_NullParameter_ReturnsFalse()
        {
            Element element = new ElementImpl(Name);
            
            Assert.That(element.Equals(null), Is.False);
        }

        [Test]
        public void Equals_ParameterNotBelongsToXmppStreamElementType_ReturnsFalse()
        {
            Element element = new ElementImpl(Name);
            object other = new DateTime();

            Assert.That(element.Equals(other), Is.False);
        }

        [Test]
        public void Equals_ElementsWithSameNamesAndWithoutNamespaces_ReturnsTrue()
        {
            Element element1 = new ElementImpl("e1");
            Element element2 = new ElementImpl("e1");

            Assert.That(element1.Equals(element2), Is.True);
        }

        [Test]
        public void Equals_ElementsWithDifferentNamesAndWithoutNamespaces_ReturnsFalse()
        {
            Element element1 = new ElementImpl("e1");
            Element element2 = new ElementImpl("e2");

            Assert.That(element1.Equals(element2), Is.False);
        }

        [Test]
        public void Equals_ElementsWithDifferendAttributes_ReturnsFalse()
        {
            ElementImpl element1 = new ElementImpl("playboy");
            ElementImpl element2 = new ElementImpl("playboy");

            element1["a1"] = "v1";
            element2["a2"] = "v2";

            Assert.That(element1.Equals(element2), Is.False);
        }
        
        [Test]
        public void Equals_ElementsWithSameAttributes_ReturnsTrue()
        {
            ElementImpl element1 = new ElementImpl("el");
            ElementImpl element2 = new ElementImpl("el");

            element1["a0"] = "v0";
            element2["a0"] = "v0";

            Assert.That(element1.Equals(element2), Is.True);
        }

        [Test]
        public void Equals_ElementsWithSameAttributeNamesAndDifferentAttributeValues_ReturnsFalse()
        {
            ElementImpl element1 = new ElementImpl("el");
            ElementImpl element2 = new ElementImpl("el");

            element1["a1"] = "v1";
            element2["a1"] = "v2";

            Assert.That(element1.Equals(element2), Is.False);
        }

        #endregion
    }
}
