using NUnit.Framework;
using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    public class PresenceTests
    {
        #region Creation methods tests

        [Test]
        public void Available_CreatesStanzaWithoutType()
        {
            VerifyCreationMethod(Presence.Available, null);
        }

        [Test]
        public void Unavailable_CreatesStanzaWithTypeUnavailable()
        {
            VerifyCreationMethod(Presence.Unavailable, Presence.Type.Unavailable);
        }

        [Test]
        public void Subscribe_CreatesStanzaWithTypeSubscribe()
        {
            VerifyCreationMethod(Presence.Subscribe, Presence.Type.Subscribe);
        }

        [Test]
        public void Subscribed_CreatesStanzaWithTypeSubscribe()
        {
            VerifyCreationMethod(Presence.Subscribed, Presence.Type.Subscribed);
        }

        [Test]
        public void Unsubscribe_CreatesStanzaWithTypeSubscribe()
        {
            VerifyCreationMethod(Presence.Unsubscribe, Presence.Type.Unsubscribe);
        }

        [Test]
        public void Unsubscribed_CreatesStanzaWithTypeSubscribe()
        {
            VerifyCreationMethod(Presence.Unsubscribed, Presence.Type.Unsubscribed);
        }

        [Test]
        public void Error_CreatesStanzaWithTypeError()
        {
            VerifyCreationMethod(Presence.Error, Presence.Type.Error);
        }

        [Test]
        public void Probe_CreatesStanzaWithTypeProbe()
        {
            VerifyCreationMethod(Presence.Probe, Presence.Type.Probe);
        }

        private void VerifyCreationMethod(Func<PresenceStanza> method, string expectedType)
        {
            PresenceStanza stanza = method();
            Assert.That(stanza.Type, Is.EqualTo(expectedType));
        }

        #endregion

        #region Is.. methods tests

        [Test]
        public void IsAvailable_NoType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsAvailable, "", true);
        }

        [Test]
        public void IsAvailable_NotEmptyType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsAvailable, "available", false);
        }

        [Test]
        public void IsUnavailable_UnavailableType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsUnavailable, Presence.Type.Unavailable, true);
        }

        [Test]
        public void IsUnvailable_NotUnavailableType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsUnavailable, "not-unavailable", false);
        }

        [Test]
        public void IsSubscribe_SubscribeType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsSubscribe, Presence.Type.Subscribe, true);
        }

        [Test]
        public void IsSubscribe_NotSubscribeType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsSubscribe, "not-subscribe", false);
        }

        [Test]
        public void IsSubscribed_SubscribedType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsSubscribed, Presence.Type.Subscribed, true);
        }

        [Test]
        public void IsSubscribed_NotSubscribeType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsSubscribed, "not-subscribed", false);
        }

        [Test]
        public void IsUnsubscribe_UnsubscribeType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsUnsubscribe, Presence.Type.Unsubscribe, true);
        }

        [Test]
        public void IsUnsubscribe_NotUnsubscribeType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsUnsubscribe, "not-unsubscribe", false);
        }

        [Test]
        public void IsUnsubscribed_UnsubscribedType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsUnsubscribed, Presence.Type.Unsubscribed, true);
        }

        [Test]
        public void IsUnsubscribe_NotUnsubscribedType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsUnsubscribed, "not-unsubscribed", false);
        }

        [Test]
        public void IsError_ErrorType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsError, Presence.Type.Error, true);
        }

        [Test]
        public void IsError_NotErrorType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsError, "not-error", false);
        }

        [Test]
        public void IsProbe_ProbeType_ReturnsTrue()
        {
            VerifyIsMethod(Presence.IsProbe, Presence.Type.Probe, true);
        }

        [Test]
        public void IsProbe_NotProbeType_ReturnsFalse()
        {
            VerifyIsMethod(Presence.IsProbe, "not-probe", false);
        }

        private void VerifyIsMethod(Func<PresenceStanza, bool> isMethod, string type, bool expected)
        {
            PresenceStanza stanza = new PresenceStanza(type);
            Assert.That(isMethod(stanza), Is.EqualTo(expected));
        }

        #endregion 

        #region Show methods tests

        [Test]
        public void Show_AnyValueOfShowProperty_ReturnsStanzaWithCorrectShowProperty()
        {
            PresenceStanza stanza = new PresenceStanza().Show("some-show-value");
            Assert.That(stanza.Show, Is.EqualTo("some-show-value"));
        }

        [Test]
        public void ShowAway_ReturnsStanzaWithCorrectShowProperty()
        {            
            VerifyShowMethod(Presence.ShowAway, Presence.ShowStatus.Away);
        }

        [Test]
        public void ShowChat_ReturnsStanzaWithCorrectShowProperty()
        {
            VerifyShowMethod(Presence.ShowChat, Presence.ShowStatus.Chat);
        }

        [Test]
        public void ShowDoNotDisturb_ReturnsStanzaWithCorrectShowProperty()
        {
            VerifyShowMethod(Presence.ShowDoNotDisturb, Presence.ShowStatus.DoNotDisturb);
        }

        [Test]
        public void ShowExtendedAway_ReturnsStanzaWithCorrectShowProperty()
        {
            VerifyShowMethod(Presence.ShowExtendedAway, Presence.ShowStatus.ExtendedAway);
        }

        #endregion

        [Test]
        public void Status_ReturnsStanzaWithCorrectStatusProperty()
        {
            PresenceStanza stanza = new PresenceStanza().Status("some-status");
            Assert.That(stanza.Status, Is.EqualTo("some-status"));
        }

        private void VerifyShowMethod(Func<PresenceStanza, PresenceStanza> showFunc, string expectedType)
        {
            PresenceStanza stanza = showFunc(new PresenceStanza());
            Assert.That(stanza.Show, Is.EqualTo(expectedType));
        }
    }
}
