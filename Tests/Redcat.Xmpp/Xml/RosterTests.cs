using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class RosterTests
    {
        [Test]
        public void Request_Creates_Roster_Request_Stanza()
        {
            object id = Guid.NewGuid();
            JID from = "from@domain.com";

            IqStanza iq = Roster.Request(id, from);

            Assert.That(iq.IsGet(), Is.True);
            Assert.That(iq.Id, Is.EqualTo(id));
            Assert.That(iq.From, Is.EqualTo(from));
            Assert.That(iq.HasChild("query", Namespaces.Roster), Is.True);
        }

        [Test]
        public void IsRosterRequest_Returns_True_For_Roster_Get_Iq()
        {
            IqStanza iq = Roster.Request();

            Assert.That(iq.IsRosterRequest(), Is.True);
        }
    }
}
