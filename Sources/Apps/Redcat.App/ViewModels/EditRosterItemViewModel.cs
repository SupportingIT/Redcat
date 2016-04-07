using MvvmCross.Core.ViewModels;
using Redcat.Xmpp;
using System;
using System.Linq;

namespace Redcat.App.ViewModels
{
    public class EditRosterItemViewModel : MvxViewModel
    {
        private XmppCommunicator communicator;

        public EditRosterItemViewModel(XmppCommunicator communicator)
        {
            this.communicator = communicator;
            UpdateItemCommand = new MvxCommand(UpdateItem);
            CloseCommand = new MvxCommand(() => Close(this));
        }

        public void Init(string jid)
        {
            JID itemJid = jid;
            RosterItem item = communicator.Roster.FirstOrDefault(r => r.Jid == itemJid);
            if (item == null) return;
            Name = item.Name;
        }

        public string Name { get; set; }

        public IMvxCommand UpdateItemCommand { get; }

        public IMvxCommand CloseCommand { get; }

        private void UpdateItem()
        {
            throw new NotImplementedException();
        }
    }
}
