using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class RosterModuleTests
    {
        private ICollection<RosterItem> roster;
        private Action<Stanza> stanzaSender;
        private RosterModule handler;
        private IqStanza iq;

        [SetUp]
        public void SetUp()
        {
            roster = new List<RosterItem>();
            stanzaSender = A.Fake<Action<Stanza>>();
            handler = new RosterModule(roster, stanzaSender);
            iq = null;
            A.CallTo(() => stanzaSender.Invoke(A<IqStanza>._)).Invokes(c =>
            {
                iq = c.GetArgument<IqStanza>(0);
            });
        }

        [Test]
        public void RequestRosterItems_Sends_Request_Roster_Stanza()
        {
            handler.RequestRosterItems();

            Assert.That(iq.IsRosterRequest(), Is.True);
        }

        [Test]
        public void AddRosterItem_Sends_Add_Roster_Item_Stanza()
        {
            JID jid = "m@h";
            handler.AddRosterItem(jid);

            Assert.That(iq.IsSet(), Is.True);
            Assert.That(iq.GetRosterItems().Single().GetAttributeValue<JID>("jid"), Is.EqualTo(jid));
        }

        [Test]
        public void RemoveRosterItem_Sends_Remove_Roster_Item_Stanza()
        {
            JID jid = "home@world";
            handler.RemoveRosterItem(jid);

            Assert.That(iq.IsSet(), Is.True);
            Assert.That(iq.GetRosterItems().Single().GetAttributeValue<JID>("jid"), Is.EqualTo(jid));
            Assert.That(iq.GetRosterItems().Single().GetAttributeValue<string>("subscription"), Is.EqualTo("remove"));
        }

        [Test]
        public void OnIqStanzaReceived_Fills_Roster_If_Roster_Result_Stanza_Received()
        {
            var items = Enumerable.Range(0, 5).Select(i =>
            {
                var item = new XmlElement($"item");
                item.SetAttributeValue("name", $"item{i}");
                item.SetAttributeValue<JID>("jid", $"jid{i}@home");
                return item;
            }).ToArray();
            IqStanza stanza = Iq.Result();
            stanza.AddRosterQuery().AddChilds(items);

            handler.OnIqStanzaReceived(stanza);

            Assert.That(roster.Count, Is.EqualTo(items.Length));
        }

        [Test]
        public void OnIqStanzaReceived_Adds_RosterItem_If_Roster_Push_Received()
        {
            JID jid = "Amy@home";
            IqStanza iq = Iq.Set();
            var item = new XmlElement("item");
            item.SetAttributeValue("name", "Amy");
            item.SetAttributeValue("jid", jid);
            iq.AddRosterQuery().AddChild(item);

            handler.OnIqStanzaReceived(iq);

            var rosterItem = roster.Single();
            Assert.That(rosterItem.Name, Is.EqualTo("Amy"));
            Assert.That(rosterItem.Jid, Is.EqualTo(jid));
        }
    }
}
