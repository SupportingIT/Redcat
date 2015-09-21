using System;
using System.Xml.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class CompositeElementTests : ElementTestBase
    {
        public CompositeElementTests() : base("composite-element", "")
        { }

        [Test]
        public void AddItem_NonZeroContentLength_WritesContentItems()
        {
            CompositeElement element = CreateDefaultCompositeElement();
            element.AddItem(new ElementImpl("child1"));
            element.AddItem(new ElementImpl("child2"));
            XElement expectedElement = CreateXElement(Name);
            expectedElement.Add(new XElement("child1"), new XElement("child2"));

            VerifyWriteOutput(expectedElement, element);
        }

        #region Find method test

        [Test]
        public void Find_ContainsElement_ReturnsCorrectElement()
        {
            CompositeElement element = CreateCompositeElement("element");
            AddChildElements(element, "e1", "e2");

            Element childElement = element.Find("e1");

            Assert.AreEqual("e1", childElement.Name);
        }

        [Test]
        public void Find_NotContainsElement_ReturnsNull()
        {
            CompositeElement element = CreateCompositeElement("element");
            AddChildElements(element, "e1", "e2");

            Element childElement = element.Find("e3");

            Assert.IsNull(childElement);
        }

        [Test]
        public void Find_TypeParameterAndContainsElement_ReturnsCorrectElement()
        {
            CompositeElement element = CreateCompositeElement("element");
            AddChildElements(element, "e1", "e2");

            Element childElement = element.Find<ElementImpl>("e2");

            Assert.AreEqual("e2", childElement.Name);
        }

        [Test]
        public void Find_TypeParameterAndNotContainsElement_ReturnsNull()
        {
            CompositeElement element = CreateCompositeElement("element");
            AddChildElements(element, "e1", "e2");

            Element childElement = element.Find<ElementImpl>("e3");

            Assert.IsNull(childElement);
        }

        private void AddChildElements(CompositeElement element, params string[] childElementsNames)
        {
            foreach (string elementName in childElementsNames)
            {
                element.AddItem(new ElementImpl(elementName));
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Find_ElementNameNull_ThrowsException()
        {
            CompositeElement element = CreateCompositeElement("e");

            element.Find(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Find_ElementNameIsEmptyString_ThrowsException()
        {
            CompositeElement element = CreateCompositeElement("e");
            element.Find(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Find_GenericParameterAndElementNameNull_ThrowsException()
        {
            CompositeElement element = CreateCompositeElement("e");
            element.Find<Element>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Find_GenericParameterAndElementNameEmptyString_ThrowsException()
        {
            CompositeElement element = CreateCompositeElement("e");
            element.Find<Element>(string.Empty);
        }

        #endregion

        [Test]
        public void Equals_SameChildSet_ReturnsTrue()
        {
            CompositeElement<string> element1 = CreateDefaultCompositeElement<string>();
            CompositeElement<string> element2 = CreateDefaultCompositeElement<string>();

            element1.AddItems("child1", "child2");
            element2.AddItems("child1", "child2");

            Assert.That(element1.Equals(element2), Is.True);
        }

        [Test]
        public void Equals_DifferentChildSet_ReturnsFalse()
        {
            CompositeElement<int> element1 = CreateDefaultCompositeElement<int>();
            CompositeElement<int> element2 = CreateDefaultCompositeElement<int>();

            element1.AddItems(0, 1);
            element2.AddItems(2, 3);

            Assert.That(element1.Equals(element2), Is.False);
        }

        private CompositeElement CreateDefaultCompositeElement()
        {
            return CreateCompositeElement(Name);
        }

        private CompositeElement CreateCompositeElement(string name)
        {
            return CreateStreamElementMock<CompositeElement>(name);
        }

        private CompositeElement<T> CreateDefaultCompositeElement<T>()
        {
            return CreateCompositeElement<T>(Name);
        }

        private CompositeElement<T> CreateCompositeElement<T>(string name)
        {
            return CreateStreamElementMock<CompositeElement<T>>(name);
        }
    }
}
