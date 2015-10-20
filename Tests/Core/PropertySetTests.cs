using NUnit.Framework;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class PropertySetTests
    {
        [Test]
        public void Get_Returns_The_Same_Value()
        {
            PropertySet propSet = new PropertySet();
            Guid value = Guid.NewGuid();
            propSet.Set("my-value", value);

            Assert.That(propSet.Get<Guid>("my-value"), Is.EqualTo(value));
        }

        [Test]
        public void Get_Returns_Default_Value_If_Type_Different()
        {
            PropertySet propSet = new PropertySet();
            string someValue = "SomeValue";
            propSet.Set("value", someValue);

            Assert.That(propSet.Get<int>("value"), Is.EqualTo(0));
        }

        [Test]
        public void Get_Returns_Default_Value_If_No_Property_Set_Before()
        {
            PropertySet propSet = new PropertySet();

            Assert.That(propSet.Get<string>("some-value"), Is.Null);
        }
    }
}
