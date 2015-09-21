using System;
using System.Text;
using NUnit.Framework;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class JidTests
    {
        private static readonly string userName = "test-user";
        private static readonly string domain = "www.domain.com";
        private static readonly string resource = "res";
        private static readonly string fullJid = userName + "@" + domain + "/" + resource;
        private static readonly int MaxJidPartLength = 1023;

        [Datapoints] private static readonly string[] incorrectUsernames = { "us/r" };

        [Datapoints] private static readonly string[] incorrectDomains = { "d@main", "domain/" };
        
        #region Username part test

        [Test, TestCaseSource("incorrectUsernames"), ExpectedException(typeof(ArgumentException))]
        public void Constructor_IncorrectCharacterInUsernamePart_ThrowsException(string incorrectUser)
        {
            JID jid = new JID(incorrectUser, domain, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_UserNameLengthMoreThan1023_ThrowsException()
        {
            string userPart = GenerateJidPart(MaxJidPartLength + 1);
            JID jid = new JID(userPart, domain, null);
        }

        #endregion

        #region Domain part tests

        [Test, TestCaseSource("incorrectDomains"), ExpectedException(typeof(ArgumentException))]
        public void Constructor_IncorrectCharacterInDomain_ThrowsException(string incorrectDomain)
        {
            JID jid = new JID("", incorrectDomain, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_DomainLengthMoreThan1023_ThrowsException()
        {
            string domainPart = GenerateJidPart(MaxJidPartLength + 1);
            JID jid = new JID(null, domainPart, null);
        }

        #endregion

        #region Resource part test

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_ResourceLengthMoreThan1023_ThrowsException()
        {
            string resourcePart = GenerateJidPart(MaxJidPartLength + 1);
            JID jid = new JID(null, domain, resourcePart);
        }

        #endregion

        private string GenerateJidPart(int length)
        {
            StringBuilder jidPart = new StringBuilder(length);
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
            {
                jidPart.Append((char)random.Next('A', 'Z'));
            }
            return jidPart.ToString();
        }

        [Test]
        public void Parse_CorrectJidString_ReturnsCorrectJidObject()
        {
            JID result = JID.Parse(fullJid);

            Assert.That(userName, Is.EqualTo(result.User));
            Assert.That(domain, Is.EqualTo(result.Domain));
            Assert.That(resource, Is.EqualTo(result.Resource));
        }

        [Test]
        public void ToString_UserNameAndResourceNotEmpty_ReturnsCorrectJidString()
        {
            JID jid = new JID(userName, domain, resource);
            string expectedJid = fullJid;

            string actualJid = jid.ToString();

            Assert.That(expectedJid, Is.EqualTo(actualJid));
        }

        [Test]
        public void Parse_NotEmptyUserNameEmptyResource_ReturnsCorrectJidObject()
        {
            string jidString = userName + "@" + domain;

            JID jid = JID.Parse(jidString);

            Assert.That(userName, Is.EqualTo(jid.User));
            Assert.That(domain, Is.EqualTo(jid.Domain));
            Assert.That(jid.Resource, Is.Empty);
        }

        [Test]
        public void Parse_UserNameAndResourceEmpty_ReturnsCorrectJidObject()
        {
            JID jid = JID.Parse(domain);

            Assert.That(domain, Is.EqualTo(jid.Domain));
            Assert.That(jid.User, Is.Empty);
            Assert.That(jid.Resource, Is.Empty);
        }

        [Test]
        public void Equals_EqualJids_ReturnsTrue()
        {
            JID jid1 = new JID("amy", "lee", "evanescence");
            JID jid2 = new JID("amy", "lee", "evanescence");

            Assert.That(jid1.Equals(jid2), Is.True);
        }

        [Test]
        public void Can_Correctly_Compare_Inequal_Jids()
        {
            JID jid1 = new JID("amy", "lee", "evanescence");
            JID jid2 = new JID("bruce", "lee", "JetQuinDo");

            Assert.That(jid1.Equals(jid2), Is.False);
        }
    }
}
