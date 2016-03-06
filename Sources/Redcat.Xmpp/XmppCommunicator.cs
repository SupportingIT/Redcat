﻿using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Redcat.Xmpp
{
    public class XmppCommunicator : SingleChannelCommunicator<IXmppChannel>
    {        
        private ICollection<RosterItem> roster;        

        private RosterHandler rosterHandler;
        private PresenceHandler presenceHandler;
        private StanzaRouter stanzaRouter;
        private SubscriptionHandler subscriptionHandler;

        public XmppCommunicator(IXmppChannelFactory factory) : base(factory)
        {
            roster = new ObservableCollection<RosterItem>();                        
            rosterHandler = new RosterHandler(roster, Send);
            presenceHandler = new PresenceHandler(Send);
            subscriptionHandler = new SubscriptionHandler(Send);
            stanzaRouter = new StanzaRouter(OnIqStanzaReceived, OnPresenceStanzaReceived, OnMessageStanzaReceived);
            stanzaRouter.SyncContext = System.Threading.SynchronizationContext.Current;
        }

        public IEnumerable<RosterItem> Roster => roster;

        public PresenceStatus PresenceStatus
        {
            get { return presenceHandler.PresenceStatus; }
            set { SetPresenceStatus(value); }
        }

        protected override void OnChannelCreated(IXmppChannel channel)
        {
            base.OnChannelCreated(channel);
            
            if (channel is IReactiveXmppChannel)
            {
                ((IReactiveXmppChannel)channel).MessageReceived += stanzaRouter.OnXmlElementReceived;
            }
        }        

        private void OnIqStanzaReceived(IqStanza iq)
        {
            rosterHandler.OnIqStanzaReceived(iq);
            subscriptionHandler.OnRosterUpdated(roster);
            IqReceived?.Invoke(this, new IqStanzaEventArgs(iq));
        }

        private void OnPresenceStanzaReceived(PresenceStanza presence)
        {
            subscriptionHandler.OnPresenceStanzaReceived(presence);
            PresenceReceived?.Invoke(this, new PresenceStanzaEventArgs(presence));
        }

        private void OnMessageStanzaReceived(MessageStanza message)
        {
            MessageReceived?.Invoke(this, new MessageStanzaEventArgs(message));
        }

        public void Send(Stanza stanza)
        {
            Channel.Send(stanza);
            OnStanzaSended(stanza);
        }

        private void OnStanzaSended(Stanza stanza)
        {
            StanzaSended?.Invoke(this, new StanzaEventArgs<Stanza>(stanza));
        }

        public void AddContact(JID jid, string name = null, bool subscribePresence = false)
        {
            rosterHandler.AddRosterItem(jid, name);
            if (subscribePresence) subscriptionHandler.RequestSubscription(jid);
        }

        public void RemoveContact(JID jid)
        {
            rosterHandler.RemoveRosterItem(jid);
        }

        public void LoadContacts()
        {
            rosterHandler.RequestRosterItems();
        }

        public void AutorizeContact(RosterItem contact)
        {
            subscriptionHandler.AcceptSubscription(contact.Jid);
        }

        public void SubscribeForPresence(RosterItem contact)
        {
            subscriptionHandler.RequestSubscription(contact.Jid);
        }

        public void SetPresenceStatus(PresenceStatus status)
        {
            presenceHandler.SetStatus(status);
        }

        public void SendMessage(JID to, string message)
        {
            Send(Message.Chat(to, message));
        }

        public void WaitIncominMessage()
        {
            var channel = Channel as IInputChannel<XmlElement>;
            if (channel == null) throw new InvalidOperationException();
            channel.Receive();
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            RemoveSubscribers(IqReceived);
            RemoveSubscribers(PresenceReceived);
            RemoveSubscribers(MessageReceived);
        }

        public event EventHandler<IqStanzaEventArgs> IqReceived;
        public event EventHandler<PresenceStanzaEventArgs> PresenceReceived;
        public event EventHandler<MessageStanzaEventArgs> MessageReceived;
        public event EventHandler<StanzaEventArgs<Stanza>> StanzaSended;
    }
}
