using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;
using System;
using System.Security.Authentication;

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
        public void CanNegotiate_Returns_True_For_Sasl_Feature()
        {
            XmlElement sasl = new XmlElement("mechanisms", Namespaces.Sasl);

            Assert.That(negotiator.CanNegotiate(sasl), Is.True);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Negotiate_Throws_Exception_If_Invalid_Feature_Provided()
        {
            XmlElement element = new XmlElement("not-mechanisms");

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Negotiate_Throws_Exception_If_No_Mechanisms()
        {
            XmlElement element = new XmlElement("mechanisms");

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Negotiate_Throws_Exception_If_No_Authenticators_For_Mechanisms()
        {
            XmlElement element = CreateSaslFeature("m1", "m2", "m3");

            negotiator.AddAuthenticator("m4", A.Fake<SaslAuthenticator>());
            negotiator.AddAuthenticator("m5", A.Fake<SaslAuthenticator>());

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        public void Negotiate_Uses_Correct_Authenticator()
        {
            XmlElement element = CreateSaslFeature("m0", "m2", "m1", "m3");
            SaslAuthenticator authenticator = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("m2", authenticator);

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => authenticator.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustHaveHappened();
        }

        [Test]
        public void Negotiate_Ignores_Mechanisms_With_Empty_Values()
        {
            XmlElement element = new XmlElement("mechanisms");
            element.Childs.Add(new XmlElement("mechanism") { Value = null });
            element.Childs.Add(new XmlElement("mechanism") { Value = "" });
            element.Childs.Add(new XmlElement("mechanism") { Value = "auth0" });
            SaslAuthenticator auth = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("auth0", auth);

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => auth.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustHaveHappened();
        }

        [Test]
        public void Negotiate_Ignores_Non_Mechanism_Elements()
        {
            XmlElement element = new XmlElement("mechanisms");
            element.Childs.Add(new XmlElement("mechanism") { Value = "auth3" });
            element.Childs.Add(new XmlElement("non-mechanism") { Value = "auth0" });
            element.Childs.Add(new XmlElement("mechanism") { Value = "auth1" });
            SaslAuthenticator auth = A.Fake<SaslAuthenticator>();
            SaslAuthenticator auth1 = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("auth0", auth);
            negotiator.AddAuthenticator("auth1", auth1);

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => auth.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustNotHaveHappened();
            A.CallTo(() => auth1.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Negotiate_Throws_Exception_If_Authentication_Failed()
        {
            SaslAuthenticator authenticator = A.Fake<SaslAuthenticator>();
            A.CallTo(() => authenticator.Invoke(A<IXmppStream>._, A<ConnectionSettings>._)).Returns(Tls.Failure);
            negotiator.AddAuthenticator("auth", authenticator);
            XmlElement element = CreateSaslFeature("auth");

            negotiator.Negotiate(A.Fake<IXmppStream>(), element);
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
