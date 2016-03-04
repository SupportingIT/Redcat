using MvvmCross.Core.ViewModels;
using Redcat.App.Services;
using Redcat.Xmpp;
using System;
using System.Collections.Generic;

namespace Redcat.App.ViewModels
{
    public class XmppCommunicatorViewModel : MvxViewModel
    {
        private IConnectionSettingsRepository repository;
        private XmppCommunicator communicator;

        public XmppCommunicatorViewModel(XmppCommunicator communicator, IConnectionSettingsRepository repository)
        {
            this.repository = repository;
            this.communicator = communicator;
            AddRosterItemCommand = new MvxCommand(AddRosterItem);
            ConnectCommand = new MvxCommand(Connect);
            RemoveRosterItemCommand = new MvxCommand<RosterItem>(RemoveRosterItem);
        }

        public IEnumerable<RosterItem> Roster => communicator.Roster;

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
}
