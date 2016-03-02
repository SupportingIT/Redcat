using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class XmppCommunicator : SingleChannelCommunicator<IXmppChannel>
    {        
        private ICollection<RosterItem> roster;
        private IDisposable subscription;

        private RosterHandler rosterHandler;        
        private StanzaRouter stanzaRouter;
        private SubscriptionHandler subscriptionHandler;

        public XmppCommunicator(IXmppChannelFactory factory) : base(factory)
        {
            roster = new List<RosterItem>();            
            stanzaRouter = new StanzaRouter();            
        }

        public IEnumerable<RosterItem> Contacts => roster;

        protected override void OnChannelCreated(IXmppChannel channel)
        {
            base.OnChannelCreated(channel);
            if (channel is IObservable<XmlElement>)
            {
                subscription = ((IObservable<XmlElement>)channel).Subscribe(stanzaRouter);
            }
            rosterHandler = new RosterHandler(roster, channel);
            subscriptionHandler = new SubscriptionHandler(channel);
            stanzaRouter.Subscribe(rosterHandler);
            stanzaRouter.Subscribe(subscriptionHandler);
        }

        public void Send(Stanza stanza) => Channel.Send(stanza);

        public void AddContact(JID jid, string name = null)
        {
            rosterHandler.AddRosterItem(jid, name);
            subscriptionHandler.RequestSubscription(jid);
        }

        public void RemoveContact(JID jid)
        {
            rosterHandler.RemoveRosterItem(jid);
        }

        public void LoadContacts()
        {
            rosterHandler.RequestRosterItems();
        }

        public void WaitIncominMessage()
        {
            var channel = Channel as IInputChannel<XmlElement>;
            if (channel == null) throw new InvalidOperationException();
            channel.Receive();
        }
    }
}
