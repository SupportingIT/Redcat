using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class RosterTests
    {
        [Test]
        public void Request_Creates_Roster_Request_Stanza()
        {
            JID from = "from@domain.com";

            IqStanza iq = Roster.Request(from);

            Assert.That(iq.IsGet(), Is.True);
            Assert.That(iq.From, Is.EqualTo(from));
            Assert.That(iq.HasChild("query", Namespaces.Roster), Is.True);
        }

        [Test]
        public void AddRosterQuery_Adds_Query_Element_With_Roster_Namespace()
        {
            IqStanza stanza = new IqStanza();
            stanza.AddRosterQuery();

            Assert.That(stanza.HasChild("query", Namespaces.Roster));
        }

        [Test]
        public void IsRosterRequest_Returns_True_For_Roster_Get_Iq()
        {
            IqStanza iq = Iq.Get();
            iq.AddRosterQuery();

            Assert.That(iq.IsRosterRequest(), Is.True);
        }

        [Test]
        public void IsRosterResponse_Returns_True_For_Roster_Result()
        {
            IqStanza iq = Iq.Result();
            iq.AddRosterQuery().AddChilds(new XmlElement("item"), new XmlElement("item"));

            Assert.That(iq.IsRosterResponse(), Is.True);
        }

        [Test]
        public void GetRosterItems_Returns_Items()
        {
            var items = Enumerable.Range(0, 5).Select(i =>
            {
                XmlElement item = new XmlElement("item");
                item.SetAttributeValue("name", $"item{i}");
                return item;
            }).ToArray();

            IqStanza stanza = new IqStanza();
            stanza.AddRosterQuery().AddChilds(items);

            CollectionAssert.AreEquivalent(items, stanza.GetRosterItems());
        }

        [Test]
        public void IsRosterPush_Returns_True_For_Roster_Push_Stanza()
        {
            IqStanza stanza = Iq.Set();
            stanza.AddRosterQuery().AddChild(new XmlElement("item"));

            Assert.That(stanza.IsRosterPush(), Is.True);
        }

        [Test]
        public void AddItem_Returns_Roster_Add_Item_Stanza()
        {
            JID itemJid = "someone@home";

            IqStanza stanza = Roster.AddItem(itemJid);

            var item = stanza.Child("query", Namespaces.Roster).Child("item");
            Assert.That(stanza.IsSet(), Is.True);
            Assert.That(item.GetAttributeValue<JID>("jid"), Is.EqualTo(itemJid));
        }

        [Test]
        public void RemoveItem_Returns_Roster_Remove_Item_Stanza()
        {
            JID itemJid = "my@home";

            IqStanza stanza = Roster.RemoveItem(itemJid);

            var item = stanza.Child("query", Namespaces.Roster).Child("item");
            Assert.That(stanza.IsSet(), Is.True);
            Assert.That(item.GetAttributeValue<JID>("jid"), Is.EqualTo(itemJid));
            Assert.That(item.GetAttributeValue<string>("subscription"), Is.EqualTo("remove"));
        }
    }
}
