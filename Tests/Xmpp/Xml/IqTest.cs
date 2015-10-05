using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class IqTest
    {
        private string xmlns = "some-ns";

        #region Creation methods tests

        [Test]
        public void Get_CreatesIqStanzaWithTypeGet()
        {
            IqStanza stanza = Iq.Get();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Get));
        }

        [Test]
        public void Set_CreatesIqStanzaWithSetType()
        {
            IqStanza stanza = Iq.Set();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Set));
        }

        [Test]
        public void Result_CreatesIqStanzaWithResultType()
        {
            IqStanza stanza = Iq.Result();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Result));
        }

        [Test]
        public void Error_CreatesIqStanzaWithErrorType()
        {
            IqStanza stanza = Iq.Error();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Error));
        }

        #endregion

        #region Is... methods tests

        [Test]
        public void IsGet_GetStanzaType_ReturnsTrue() 
        {
            VerifyIsMethod(Iq.IsGet, Iq.Type.Get, true);
        }

        [Test]
        public void IsGet_NotGetStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsGet, "not-get", false);
        }

        [Test]
        public void IsSet_SetStanzaType_ReturnsTrue()
        {
            VerifyIsMethod(Iq.IsSet, Iq.Type.Set, true);
        }

        [Test]
        public void IsSet_NotSetStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsSet, "not-set", false);
        }

        [Test]
        public void IsResult_ResultStanzaType_ReturnsTrue()
        {
            VerifyIsMethod(Iq.IsResult, Iq.Type.Result, true);
        }

        [Test]
        public void IsResult_NotResultStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsResult, "not-result", false);
        }

        [Test]
        public void IsError_ErrorStanzaType_ReturnsTrue()
        {
            VerifyIsMethod(Iq.IsError, Iq.Type.Error, true);
        }

        [Test]
        public void IsError_NotErrorStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsError, "not-error", false);
        }

        private void VerifyIsMethod(Func<IqStanza, bool> isFunc, string type, bool expected)
        {
            IqStanza stanza = new IqStanza(type);
            Assert.That(isFunc(stanza), Is.EqualTo(expected));
        }

        #endregion

        #region Query method tests

        [Test]
        public void Query_StanzaContainsQueryElement_ReturnsQueryElement()
        {
            IqStanza iqStanza = new IqStanza();
            IqQuery queryElement = new IqQuery(xmlns);
            
            IqQuery actualElement = iqStanza.Query();

            Assert.AreEqual(queryElement, actualElement);
        }

        [Test]
        public void Query_StanzaNotContainsQueryElement_ReturnsNull()
        {
            IqStanza iqStanza = new IqStanza();

            IqQuery actualElement = iqStanza.Query();

            Assert.That(actualElement, Is.Null);
        }

        [Test]
        public void QueryXmlns_IqStanza_AddsQueryElementWithGivenXmlns()
        {
            IqStanza stanza = new IqStanza().Query(xmlns);
            IqQuery query = null;//stanza.Find<IqQuery>("query");

            Assert.That(query.Xmlns, Is.EqualTo(xmlns));
        }

        [Test]
        public void QueryXmlns_()
        {
            RosterItem[] childItems = CreateChildItems();

            IqStanza stanza = new IqStanza().Query(xmlns, childItems);

            IqQuery query = stanza.Query();
            Assert.Fail();
            //CollectionAssert.AreEquivalent(query.Items, childItems);
        }

        [Test]
        public void QueryXmlns__()
        {
            RosterItem[] childItems = CreateChildItems();

            //IqStanza stanza = new IqStanza().Query(xmlns, q => q.AddItems(childItems));

            //IqQuery query = stanza.Query();
            //CollectionAssert.AreEquivalent(query.Items, childItems);
            Assert.Fail();
        }

        #endregion

        #region RosterQuery method tests

        [Test]
        public void RosterQuery_AddsQueryElementWithRosterXmlns()
        {
            IqStanza stanza = new IqStanza().RosterQuery();
            IqQuery query = stanza.Query();

            //Assert.That(query.Xmlns, Is.EqualTo(XmppNamespaces.Roster));
        }

        [Test]
        public void RosterQuery_AddsQueryElementWithGivenChildItems()
        {
            RosterItem[] childItems = CreateChildItems();

            IqStanza stanza = null;//new IqStanza().RosterQuery(q => q.AddItems(childItems));

            IqQuery query = stanza.Query();
            //CollectionAssert.AreEquivalent(query.Items, childItems);
            Assert.Fail();
        }

        public void RosterQuery_AddsQueryElementAndCallsInitFunc()
        {
            RosterItem[] childItems = CreateChildItems();

            IqStanza stanza = new IqStanza().RosterQuery(childItems);

            IqQuery query = stanza.Query();
            //CollectionAssert.AreEquivalent(query.Items, childItems);
            Assert.Fail();
        }

        private RosterItem[] CreateChildItems(int count = 3)
        {
            return Enumerable.Range(0, count)
                             .Select(i => new RosterItem("item" + i, "item" + i + "@jid.org"))
                             .ToArray();
        }

        #endregion

        #region HasQuery method tests

        [Test]
        public void HasQuery_QueryElementXmlnsEqualsToGivenParameter_ReturnTrue()
        {
            IqStanza iq = new IqStanza();
            //iq.AddItem(new IqQuery(xmlns));

            Assert.That(iq.HasQuery(xmlns), Is.True);
        }

        [Test]
        public void HasQuery_QueryElementXmlnsNotEqualsToGivenParameter_ReturnFalse()
        {
            IqStanza iq = new IqStanza();
            iq.Query("some-xmlns");

            Assert.That(iq.HasQuery("some-other-xmlns"), Is.False);
        }

        [Test]
        public void HasQuery_NoQueryElementInContent_ReturnsFalse()
        {
            IqStanza iq = new IqStanza();

            Assert.That(iq.HasQuery(), Is.False);
        }

        #endregion

        #region IsRosterResult tests

        [Test]
        public void IsRosterResult_InvalidQueryNamspaceAndIqType_ReturnFalse()
        {
            IqStanza iq = CreateIq(Iq.Type.Get, "invalid-roster-ns");

            Assert.That(iq.IsRosterResult(), Is.False);
        }

        [Test]
        public void IsRosterResult_InvalidQueryNamspaceAndValidIqType_ReturnFalse()
        {
            IqStanza iq = CreateIq(Iq.Type.Result, "invalid-roster-ns");

            Assert.That(iq.IsRosterResult(), Is.False);
        }

        [Test]
        public void IsRosterResult_ValidQueryNamspaceAndInvalidIqType_ReturnFalse()
        {
            IqStanza iq = CreateIq(Iq.Type.Error, Namespaces.Roster);

            Assert.That(iq.IsRosterResult(), Is.False);
        }

        [Test]
        public void IsRosterResult_ValidQueryNamspaceAndIqType_ReturnTrue()
        {
            IqStanza iq = CreateIq(Iq.Type.Result, Namespaces.Roster);

            Assert.That(iq.IsRosterResult(), Is.True);
        }

        #endregion

        [Test]
        public void HasRosterQuery_QueryElementInRosterNamespace_ReturnsTrue()
        {
            IqStanza iqStanza = CreateIq("", Namespaces.Roster);

            bool hasRosterQuery = iqStanza.HasRosterQuery();
            Assert.That(hasRosterQuery, Is.True);
        }

        [Test]
        public void HasRosterQuery_QueryElementNotInRosterNamespace_ReturnsTrue()
        {
            IqStanza iqStanza = CreateIq("", "not-roster-ns");

            bool hasRosterQuery = iqStanza.HasRosterQuery();
            Assert.That(hasRosterQuery, Is.False);
        }

        [Test]
        public void RosterItems_IqContainsRosterItems_ReturnsRosterItemsElements()
        {
            RosterItem[] items = new[] {new RosterItem("item1"), new RosterItem("item2") };
            IqStanza iqStanza = CreateRosterResultIq(items);

            ICollection<RosterItem> actualItems = iqStanza.RosterItems();

            CollectionAssert.AreEquivalent(items, actualItems);
        }

        [Test]
        public void RosterItems_IqNotContainRosterItems_ReturnsEmptyCollection()
        {
            IqStanza iqStanza = new IqStanza();

            ICollection<RosterItem> actualItems = iqStanza.RosterItems();

            CollectionAssert.IsEmpty(actualItems);
        }

        public IqStanza CreateRosterResultIq(IEnumerable<RosterItem> items)
        {
            IqStanza result = CreateIq(Iq.Type.Result, Namespaces.Roster);
            IqQuery queryElement = result.Query();
            foreach (RosterItem item in items)
            {
                //queryElement.AddItem(item);
            }
            return result;
        }

        private IqStanza CreateIq(string type, string queryXmlns)
        {
            IqStanza iqStanza = new IqStanza(type);
            iqStanza.Query(queryXmlns);
            return iqStanza;
        }
    }
}