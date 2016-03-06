using MvvmCross.Core.ViewModels;
using Redcat.App.Services;
using Redcat.Xmpp;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Redcat.App.ViewModels
{
    public class XmppCommunicatorViewModel : MvxViewModel
    {
        private IConnectionSettingsRepository repository;        
        private XmppCommunicator communicator;
        private PresenceStatusViewModel selectedPresence;
        private ICollection<RosterItemViewModel> rosterItems;
        private ICollection<XmppElementViewModel> streamItems;

        public XmppCommunicatorViewModel(XmppCommunicator communicator, IConnectionSettingsRepository repository)
        {
            this.repository = repository;
            this.communicator = communicator;
            streamItems = new ObservableCollection<XmppElementViewModel>();
            rosterItems = new ObservableCollection<RosterItemViewModel>();
            AttachEventHandlers(communicator);
            InitializeCommands();
            InitializePresenceProperties();
            if (communicator.Roster is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)communicator.Roster).CollectionChanged += (s, e) =>
                {
                    streamItems.Clear();
                    foreach(var item in (IEnumerable<RosterItem>)s)
                    {
                        rosterItems.Add(new RosterItemViewModel(item));
                    }
                };
            }
        }

        private void AttachEventHandlers(XmppCommunicator communicator)
        {
            communicator.IqReceived += (s, args) => streamItems.Add(XmppElementViewModel.Inbound(args.Stanza));
            communicator.PresenceReceived += (s, args) => streamItems.Add(XmppElementViewModel.Inbound(args.Stanza));
            communicator.MessageReceived += (s, args) => streamItems.Add(XmppElementViewModel.Inbound(args.Stanza));
            communicator.StanzaSended += (s, args) => streamItems.Add(XmppElementViewModel.Outbound(args.Stanza));
        }

        private void InitializeCommands()
        {
            ConnectCommand = new MvxCommand(Connect);
            AddRosterItemCommand = new MvxCommand(AddRosterItem);            
            RemoveRosterItemCommand = new MvxCommand<RosterItem>(RemoveRosterItem);
            ApproveInboundSubscriptionCommand = new MvxCommand<RosterItem>(ApproveInboundSubscription);
            SubscribeForPresenceCommand = new MvxCommand<RosterItem>(SubscribeForPresence);
        }

        private void InitializePresenceProperties()
        {
            PresenceStatuses = GetPresenceStatuses();
            SelectedPresence = PresenceStatuses.First();
        }

        private IEnumerable<PresenceStatusViewModel> GetPresenceStatuses()
        {
            return new PresenceStatusViewModel[]
            {
                new PresenceStatusViewModel("Offline", PresenceStatus.Unavaliable),
                new PresenceStatusViewModel("Available", PresenceStatus.Available),
                new PresenceStatusViewModel("Away", PresenceStatus.Away),
                new PresenceStatusViewModel("Chat", PresenceStatus.Chat)
            };
        }

        public bool IsConnected => communicator.IsConnected;

        public IEnumerable<RosterItem> Roster => communicator.Roster;

        public PresenceStatusViewModel SelectedPresence
        {
            get { return selectedPresence; }
            set
            {
                selectedPresence = value;
                if (communicator.IsConnected) communicator.SetPresenceStatus(selectedPresence.Status);
            }
        }

        public IEnumerable<PresenceStatusViewModel> PresenceStatuses { get; private set; }

        public IEnumerable<XmppElementViewModel> StreamItems => streamItems;

        public IMvxCommand ConnectCommand { get; private set; }

        public IMvxCommand AddRosterItemCommand { get; private set; }

        public IMvxCommand RemoveRosterItemCommand { get; private set; }

        public IMvxCommand ApproveInboundSubscriptionCommand { get; private set; }

        public IMvxCommand SubscribeForPresenceCommand { get; private set; }

        private void Connect()
        {
            communicator.Connect(repository.Get());
            RaisePropertyChanged(nameof(IsConnected));
            communicator.LoadContacts();
            communicator.SetPresenceStatus(SelectedPresence.Status);            
        }

        private void AddRosterItem()
        {
            ShowViewModel<AddRosterItemViewModel>();
        }

        private void RemoveRosterItem(RosterItem item)
        {
            //communicator.RemoveContact(item);
        }

        private void ApproveInboundSubscription(RosterItem item)
        {
            communicator.AutorizeContact(item);
        }

        private void SubscribeForPresence(RosterItem item)
        {
            communicator.SubscribeForPresence(item);
        }
    }

    public class RosterItemViewModel
    {
        private RosterItem item;

        public RosterItemViewModel(RosterItem item)
        {
            this.item = item;
            ApproveSubscriptionCommand = new MvxCommand(() => { throw new NotImplementedException(); });
            SubscribeForPresenceCommand = new MvxCommand(() => { throw new NotImplementedException(); });
        }

        public string Name => item.Name;

        public JID Jid => item.Jid;

        public SubscriptionState SubscriptionState => item.SubscriptionState;

        public IMvxCommand ApproveSubscriptionCommand { get; }

        public IMvxCommand SubscribeForPresenceCommand { get; }
    }

    public class PresenceStatusViewModel
    {
        public PresenceStatusViewModel(string displayText, PresenceStatus status)
        {
            DisplayText = displayText;
            Status = status;
        }

        public string DisplayText { get; }
        public PresenceStatus Status { get; }
    }

    public class XmppElementViewModel
    {
        private XmlElement element;
        
        public XmppElementViewModel(XmlElement element, XmppStreamDirection direction)
        {
            this.element = element;
            Direction = direction;
        }

        public XmppStreamDirection Direction { get; }

        public string Name => element.Name;

        protected Stanza Stanza => element as Stanza;

        public string Type => Stanza?.Type;

        public object Id => Stanza?.Id;

        public JID From => Stanza?.From;

        public JID To => Stanza?.To;

        public static XmppElementViewModel Inbound(XmlElement element) => new XmppElementViewModel(element, XmppStreamDirection.Inbound);
        public static XmppElementViewModel Outbound(XmlElement element) => new XmppElementViewModel(element, XmppStreamDirection.Outbound);
    }

    public enum XmppStreamDirection { Inbound, Outbound }
}
