using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

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
            roster = new ObservableCollection<RosterItem>();
            stanzaRouter = new StanzaRouter();            
        }

        public IEnumerable<RosterItem> Roster => roster;

        protected override void OnChannelCreated(IXmppChannel channel)
        {
            base.OnChannelCreated(channel);
            if (channel is IObservable<XmlElement>)
            {
                subscription = ((IObservable<XmlElement>)channel).Subscribe(stanzaRouter);
            }
            rosterHandler = new RosterHandler(roster, channel) { SyncContext = SynchronizationContext.Current };
            subscriptionHandler = new SubscriptionHandler(channel);
            stanzaRouter.Subscribe(rosterHandler);
            stanzaRouter.Subscribe(subscriptionHandler);
        }

        public void Send(Stanza stanza) => Channel.Send(stanza);

        public void AddContact(RosterItem contact)
        {
            AddContact(contact.Jid, contact.Name);
        }

        public void AddContact(JID jid, string name = null)
        {
            rosterHandler.AddRosterItem(jid, name);            
        }

        public void RemoveContact(RosterItem contact)
        {
            RemoveContact(contact.Jid, contact.Name);
        }

        public void RemoveContact(JID jid, string name = null)
        {
            rosterHandler.RemoveRosterItem(jid, name);
        }

        public void LoadRoster()
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
