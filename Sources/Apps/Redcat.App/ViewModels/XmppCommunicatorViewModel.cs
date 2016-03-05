using MvvmCross.Core.ViewModels;
using Redcat.App.Services;
using Redcat.Xmpp;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Redcat.App.ViewModels
{
    public class XmppCommunicatorViewModel : MvxViewModel
    {
        private IConnectionSettingsRepository repository;
        private ICollection<XmppStreamItem> streamItems;
        private XmppCommunicator communicator;

        public XmppCommunicatorViewModel(XmppCommunicator communicator, IConnectionSettingsRepository repository)
        {
            this.repository = repository;
            this.communicator = communicator;
            AddRosterItemCommand = new MvxCommand(AddRosterItem);
            ConnectCommand = new MvxCommand(Connect);
            RemoveRosterItemCommand = new MvxCommand<RosterItem>(RemoveRosterItem);
            streamItems = new ObservableCollection<XmppStreamItem>();            
        }        

        public IEnumerable<XmppStreamItem> StreamItems => streamItems;

        public IMvxCommand ConnectCommand { get; }

        public IMvxCommand AddRosterItemCommand { get; }

        public IMvxCommand RemoveRosterItemCommand { get; }
        
        private void Connect()
        {
            communicator.Connect(repository.Get());
            communicator.LoadRoster();
        }

        private void AddRosterItem()
        {
            ShowViewModel<AddRosterItemViewModel>();
        }

        private void RemoveRosterItem(RosterItem item)
        {
            communicator.RemoveContact(item);
        }
    }

    public class XmppStreamItem
    {
        //private XmlElement element;
        
        public XmppStreamItem(XmppStreamDirection direction)
        {
            //this.element = element;
            Direction = direction;
        }

        public XmppStreamDirection Direction { get; }

        //public string Name => element.Name;
    }

    public enum XmppStreamDirection { Inbound, Outbound }
}
