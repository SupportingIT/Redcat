using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests.Negotiators
{
    [TestFixture]
    public class SaslNegotiatorTests
    {
        private ConnectionSettings settings;
        private SaslNegotiator negotiator;

        [SetUp]
        public void SetUp()
        {
            settings = new ConnectionSettings();
            negotiator = new SaslNegotiator(settings);
        }

        [Test]
        public void CanNegoatiate_Returns_True_For_Sasl_Feature()
        {
            XmlElement sasl = new XmlElement("mechanisms", Namespaces.Sasl);

            Assert.That(negotiator.CanNeogatiate(sasl), Is.True);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Neogatiate_Throws_Exception_If_Invalid_Feature_Provided()
        {
            XmlElement element = new XmlElement("not-mechanisms");

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Neogotiate_Throws_Exception_If_No_Mechanisms()
        {
            XmlElement element = new XmlElement("mechanisms");

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Neogatiate_Throws_Exception_If_No_Authenticators_For_Mechanisms()
        {
            XmlElement element = CreateSaslFeature("m1", "m2", "m3");

            negotiator.AddAuthenticator("m4", A.Fake<SaslAuthenticator>());
            negotiator.AddAuthenticator("m5", A.Fake<SaslAuthenticator>());

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        public void Negoatiate_Uses_Correct_Authenticator()
        {
            XmlElement element = CreateSaslFeature("m0", "m2", "m1", "m3");
            SaslAuthenticator authenticator = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("m2", authenticator);

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => authenticator.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustHaveHappened();
        }

        [Test]
        public void Negoatiate_Ignores_Mechanisms_With_Empty_Values()
        {
            XmlElement element = new XmlElement("mechanisms");
            element.Childs.Add(new XmlElement("mechanism") { Value = null });
            element.Childs.Add(new XmlElement("mechanism") { Value = "" });
            element.Childs.Add(new XmlElement("mechanism") { Value = "auth0" });
            SaslAuthenticator auth = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("auth0", auth);

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => auth.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustHaveHappened();
        }

        [Test]
        public void Neogatiate_Ignores_Non_Mechanism_Elements()
        {
            XmlElement element = new XmlElement("mechanisms");
            element.Childs.Add(new XmlElement("mechanism") { Value = "auth3" });
            element.Childs.Add(new XmlElement("non-mechanism") { Value = "auth0" });
            element.Childs.Add(new XmlElement("mechanism") { Value = "auth1" });
            SaslAuthenticator auth = A.Fake<SaslAuthenticator>();
            SaslAuthenticator auth1 = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("auth0", auth);
            negotiator.AddAuthenticator("auth1", auth1);

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => auth.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustNotHaveHappened();
            A.CallTo(() => auth1.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustHaveHappened();
        }

        private XmlElement CreateSaslFeature(params string[] mechanisms)
        {
            XmlElement element = new XmlElement("mechanisms");

            foreach(var mechanism in mechanisms)
            {
                XmlElement auth = new XmlElement("mechanism") { Value = mechanism };
                element.Childs.Add(auth);
            }

            return element;
        }
    }
}
