using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppCommunicatorTests
    {        
        private IReactiveXmppChannel channel;
        private IXmppChannelFactory factory;
        private XmppCommunicator communicator;
        private JID jid = "user@home";

        [SetUp]
        public void SetUp()
        {
            channel = A.Fake<IReactiveXmppChannel>();
            factory = A.Fake<IXmppChannelFactory>();
            A.CallTo(() => factory.CreateChannel(A<ConnectionSettings>._)).Returns(channel);
            communicator = new XmppCommunicator(factory);
            communicator.Connect(new ConnectionSettings());
        }

        [Test]
        public void Send_Sends_Stanza_To_Channel()
        {
            Stanza stanza = new Stanza("stanza");

            communicator.Send(stanza);

            A.CallTo(() => channel.Send(stanza)).MustHaveHappened();
        }

        [Test]
        public void AddContact_Sends_Roster_Add_Stanza()
        {
            IqStanza rosterIq = null;
            A.CallTo(() => channel.Send(A<XmlElement>._)).Invokes(c => 
            {
                rosterIq = (IqStanza)c.GetArgument<XmlElement>(0);
            });
            communicator.AddRosterItem(jid, "User");

            Assert.That(rosterIq.IsRosterIq(), Is.True);
        }

        [Test]
        public void RemoveContact_Sends_Remove_Contact_Iq()
        {
            IqStanza rosterIq = null;
            A.CallTo(() => channel.Send(A<XmlElement>._)).Invokes(c =>
            {
                rosterIq = (IqStanza)c.GetArgument<XmlElement>(0);
            });
            communicator.RemoveRosterItem(jid);

            Assert.That(rosterIq.IsRosterIq(), Is.True);
        }

        [Test]
        public void LoadContacts_Sends_RosterRequest_Stanza()
        {
            IqStanza rosterIq = null;
            A.CallTo(() => channel.Send(A<XmlElement>._)).Invokes(c =>
            {
                rosterIq = (IqStanza)c.GetArgument<XmlElement>(0);
            });
            communicator.LoadRoster();

            Assert.That(rosterIq.IsRosterRequest(), Is.True);
        }

        #region Event tests

        [Test]
        public void Rises_IqReceived_Event()
        {            
            bool wasRised = false;
            communicator.IqReceived += (s, iq) => wasRised = true;

            RiseStanzaEvent(Iq.Set());

            Assert.That(wasRised, Is.True);
        }

        [Test]
        public void Raises_PresenceReceived_Event()
        {
            bool wasRised = false;
            communicator.PresenceReceived += (s, presence) => wasRised = true;

            RiseStanzaEvent(Presence.Available());

            Assert.That(wasRised, Is.True);
        }

        [Test]
        public void Raises_MessageReceived_Event()
        {
            bool wasRised = false;
            communicator.MessageReceived += (s, message) => wasRised = true;

            RiseStanzaEvent(new MessageStanza());

            Assert.That(wasRised, Is.True);
        }

        private void RiseStanzaEvent(Stanza stanza)
        {
            channel.MessageReceived += Raise.With(new ChannelMessageEventArgs<XmlElement>(stanza));
        }

        #endregion
    }
}
