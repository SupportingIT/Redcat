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
        [Test]
        public void CanNegoatiate_Returns_True_For_Sasl_Feature()
        {
            SaslNegotiator negoatiator = new SaslNegotiator();
            XmlElement sasl = new XmlElement("mechanisms", Namespaces.Sasl);

            Assert.That(negoatiator.CanNeogatiate(sasl), Is.True);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Neogatiate_Throws_Exception_If_Invalid_Feature_Provided()
        {
            SaslNegotiator negotiator = new SaslNegotiator();
            XmlElement element = new XmlElement("not-mechanisms");

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Neogotiate_Throws_Exception_If_No_Mechanisms()
        {
            SaslNegotiator negotiator = new SaslNegotiator();
            XmlElement element = new XmlElement("mechanisms");

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Neogatiate_Throws_Exception_If_No_Authenticators_For_Mechanisms()
        {
            SaslNegotiator negoatiator = new SaslNegotiator();
            XmlElement element = CreateSaslFeature("m1", "m2", "m3");

            negoatiator.AddAuthenticator("m4", A.Fake<SaslAuthenticator>());
            negoatiator.AddAuthenticator("m5", A.Fake<SaslAuthenticator>());

            negoatiator.Neogatiate(A.Fake<IXmppStream>(), element);
        }

        [Test]
        public void Negoatiate_Correct_Authenticator()
        {
            SaslNegotiator negotiator = new SaslNegotiator();
            XmlElement element = CreateSaslFeature("m0", "m2", "m1", "m3");
            SaslAuthenticator authenticator = A.Fake<SaslAuthenticator>();
            negotiator.AddAuthenticator("m2", authenticator);

            negotiator.Neogatiate(A.Fake<IXmppStream>(), element);

            A.CallTo(() => authenticator.Invoke(A<IXmppStream>._, A<ConnectionSettings>._));
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
