using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class XmlElementExtensionsTests
    {
        [Test]
        public void HasElement_Returns_True_If_Sequence_Contains_Element_With_Given_Name()
        {
            VerifyHasElement(true, i => new XmlElement($"element{i}"), "element2");
        }

        [Test]
        public void HasElement_Returns_True_If_Sequence_Contains_Element_With_Given_Name_And_Namespace()
        {
            VerifyHasElement(false, i => new XmlElement($"name", $"namespace{i}"), "namespace3");
        }

        [Test]
        public void HasElement_Returns_False_If_Sequence_Does_Not_Contains_Element_With_Given_Name()
        {
            VerifyHasElement(false, i => new XmlElement($"element{i}"), "element8");
        }

        [Test]
        public void HasElement_Returns_False_If_Sequence_Does_Not_Contains_Element_With_Given_Namespace()
        {
            VerifyHasElement(false, i => new XmlElement($"element", $"namespace{i}"), "element", "namespace9");
        }

        private void VerifyHasElement(bool expected, Func<int, XmlElement> newElement, string name, string xmlns = null)
        {
            var elements = Enumerable.Range(0, 3).Select(i => newElement(i));
            bool hasElement = elements.HasElement(name, xmlns);
            Assert.That(hasElement, Is.EqualTo(expected));
        }

        [Test]
        public void Element_Returns_Element_With_Given_Name()
        {
            var elements = Enumerable.Range(0, 4).Select(i => new XmlElement($"name{i}"));

            var element = elements.Element("name2");

            Assert.That(element.Name, Is.EqualTo("name2"));
        }

        [Test]
        public void Element_Returns_Null_If_No_Elements_With_Given_Name()
        {
            var elements = Enumerable.Range(0, 4).Select(i => new XmlElement($"name{i}"));

            var element = elements.Element("some-element");

            Assert.That(element, Is.Null);
        }

        [Test]
        public void Element_Returns_Element_With_Given_Name_And_Namespace()
        {
            var elements = Enumerable.Range(0, 4).Select(i => new XmlElement("elem", $"namespace{i}"));

            var element = elements.Element("elem", "namespace2");

            Assert.That(element.Name, Is.EqualTo("elem"));
            Assert.That(element.Xmlns, Is.EqualTo("namespace2"));
        }

        [Test]
        public void Element_Returns_Null_If_No_Element_With_Given_Name_And_Namespace()
        {
            var elements = Enumerable.Range(0, 4).Select(i => new XmlElement("elem", $"namespace{i}"));

            var element = elements.Element("elem", "namespace89");

            Assert.That(element, Is.Null);
        }

        [Test]
        public void HasChild_Returns_True_If_Sequence_Contains_Element_With_Given_Name_And_Namespace()
        {
            var element = new XmlElement("name")
            {
                Childs = { new XmlElement("child1", "namespace1"), new XmlElement("child2", "namespace") }
            };

            var hasChild = element.HasChild("child2", "namespace");

            Assert.That(hasChild, Is.True);
        }

        [Test]
        public void HasChild_Returns_False_If_Sequence_Does_Not_Contains_Element_With_Given_Namespace()
        {
            var element = new XmlElement("name")
            {
                Childs = { new XmlElement("child", "namespace"), new XmlElement("child2", "1namespace") }
            };

            var hasChild = element.HasChild("child2", "namespace");

            Assert.That(hasChild, Is.False);
        }

        [Test]
        public void Child_Returns_Null_If_No_Child_Element_With_Given_Name_And_Namespace()
        {
            var element = new XmlElement("name")
            {
                Childs = { new XmlElement("child", "namespace"), new XmlElement("child2", "1namespace") }
            };

            var child = element.Child("child", "namespace89");

            Assert.That(child, Is.Null);
        }

        [Test]
        public void Child_Returns_Child_Element_With_Given_Name_And_Namespace()
        {
            var element = new XmlElement("name")
            {
                Childs = { new XmlElement("child0", "namespace0"), new XmlElement("child2", "2namespace") }
            };

            var child = element.Child("child0", "namespace0");

            Assert.That(child.Name, Is.EqualTo("child0"));
            Assert.That(child.Xmlns, Is.EqualTo("namespace0"));
        }
    }
}
