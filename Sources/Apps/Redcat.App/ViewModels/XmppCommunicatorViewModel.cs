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
        private PresenceStatusItem selectedPresence;
        private ICollection<XmppStreamItem> streamItems;

        public XmppCommunicatorViewModel(XmppCommunicator communicator, IConnectionSettingsRepository repository)
        {
            this.repository = repository;
            this.communicator = communicator;
            streamItems = new ObservableCollection<XmppStreamItem>();
            AttachEventHandlers(communicator);
            InitializeCommands();
            InitializePresenceProperties();            
        }

        private void AttachEventHandlers(XmppCommunicator communicator)
        {
            communicator.IqReceived += (s, args) => streamItems.Add(XmppStreamItem.Inbound(args.Stanza));
            communicator.PresenceReceived += (s, args) => streamItems.Add(XmppStreamItem.Inbound(args.Stanza));
            communicator.MessageReceived += (s, args) => streamItems.Add(XmppStreamItem.Inbound(args.Stanza));
            communicator.StanzaSended += (s, args) => streamItems.Add(XmppStreamItem.Outbound(args.Stanza));
        }

        private void InitializeCommands()
        {
            AddRosterItemCommand = new MvxCommand(AddRosterItem);
            ConnectCommand = new MvxCommand(Connect);
            RemoveRosterItemCommand = new MvxCommand<RosterItem>(RemoveRosterItem);
        }

        private void InitializePresenceProperties()
        {
            PresenceStatuses = GetPresenceStatuses();
            SelectedPresence = PresenceStatuses.First();
        }

        private IEnumerable<PresenceStatusItem> GetPresenceStatuses()
        {
            return new PresenceStatusItem[]
            {
                new PresenceStatusItem("Offline", PresenceStatus.Unavaliable),
                new PresenceStatusItem("Available", PresenceStatus.Available),
                new PresenceStatusItem("Away", PresenceStatus.Away),
                new PresenceStatusItem("Chat", PresenceStatus.Chat)
            };
        }

        public bool IsConnected => communicator.IsConnected;

        public IEnumerable<RosterItem> Roster => communicator.Roster;

        public PresenceStatusItem SelectedPresence
        {
            get { return selectedPresence; }
            set
            {
                selectedPresence = value;
                if (communicator.IsConnected) communicator.SetPresenceStatus(selectedPresence.Status);
            }
        }

        public IEnumerable<PresenceStatusItem> PresenceStatuses { get; private set; }

        public IEnumerable<XmppStreamItem> StreamItems => streamItems;

        public IMvxCommand ConnectCommand { get; private set; }

        public IMvxCommand AddRosterItemCommand { get; private set; }

        public IMvxCommand RemoveRosterItemCommand { get; private set; }
        
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
    }

    public class PresenceStatusItem
    {
        public PresenceStatusItem(string displayText, PresenceStatus status)
        {
            DisplayText = displayText;
            Status = status;
        }

        public string DisplayText { get; }
        public PresenceStatus Status { get; }
    }

    public class XmppStreamItem
    {
        private XmlElement element;
        
        public XmppStreamItem(XmlElement element, XmppStreamDirection direction)
        {
            this.element = element;
            Direction = direction;
        }

        public XmppStreamDirection Direction { get; }

        public string Name => element.Name;

        public static XmppStreamItem Inbound(XmlElement element) => new XmppStreamItem(element, XmppStreamDirection.Inbound);
        public static XmppStreamItem Outbound(XmlElement element) => new XmppStreamItem(element, XmppStreamDirection.Outbound);
    }

    public enum XmppStreamDirection { Inbound, Outbound }
}
