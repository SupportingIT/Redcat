using MvvmCross.Core.ViewModels;
using Redcat.App.Services;
using Redcat.Xmpp;
using Redcat.Xmpp.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Redcat.App.ViewModels
{
    public class XmppCommunicatorViewModel : MvxViewModel
    {
        private IConnectionSettingsRepository repository;        
        private XmppCommunicator communicator;
        private PresenceStatusViewModel selectedPresence;        
        private ICollection<XmppElementViewModel> streamItems;

        public XmppCommunicatorViewModel(XmppCommunicator communicator, IConnectionSettingsRepository repository)
        {
            this.repository = repository;
            this.communicator = communicator;
            streamItems = new ObservableCollection<XmppElementViewModel>();            
            AttachEventHandlers(communicator);
            InitializeCommands();
            InitializePresenceProperties();            
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
            LoadRosterCommand = new MvxCommand(LoadRoster);
            AddRosterItemCommand = new MvxCommand(AddRosterItem);
            EditRosterItemCommand = new MvxCommand(EditRosterItem);
            RemoveRosterItemCommand = new MvxCommand(RemoveRosterItem);
            ApproveInboundSubscriptionCommand = new MvxCommand(ApproveInboundSubscription);
            SubscribeForPresenceCommand = new MvxCommand(SubscribeForPresence);
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

        public RosterItem SelectedRosterItem { get; set; }

        public IEnumerable<PresenceStatusViewModel> PresenceStatuses { get; private set; }

        public IEnumerable<XmppElementViewModel> StreamItems => streamItems;

        public IMvxCommand ConnectCommand { get; private set; }

        public IMvxCommand LoadRosterCommand { get; private set; }

        public IMvxCommand AddRosterItemCommand { get; private set; }

        public IMvxCommand EditRosterItemCommand { get; private set; }

        public IMvxCommand RemoveRosterItemCommand { get; private set; }        

        public IMvxCommand ApproveInboundSubscriptionCommand { get; private set; }

        public IMvxCommand SubscribeForPresenceCommand { get; private set; }

        private void Connect()
        {
            communicator.Connect(repository.Get());
            RaisePropertyChanged(nameof(IsConnected));            
        }

        private void LoadRoster()
        {
            communicator.LoadRoster();
        }

        private void AddRosterItem()
        {
            ShowViewModel<AddRosterItemViewModel>();
        }

        private void EditRosterItem()
        {
            if (SelectedRosterItem == null) return;            
            ShowViewModel<EditRosterItemViewModel>(new { jid = SelectedRosterItem.Jid.ToString() });
        }

        private void RemoveRosterItem()
        {
            if (SelectedRosterItem == null) return;
            communicator.RemoveRosterItem(SelectedRosterItem.Jid);
        }

        private void ApproveInboundSubscription()
        {
            if (SelectedRosterItem == null) return;
            communicator.ApproveInboundSubscription(SelectedRosterItem);            
        }

        private void SubscribeForPresence()
        {
            if (SelectedRosterItem == null) return;
            communicator.SubscribeForPresence(SelectedRosterItem);
        }
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
