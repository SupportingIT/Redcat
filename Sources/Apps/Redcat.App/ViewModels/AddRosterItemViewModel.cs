using MvvmCross.Core.ViewModels;
using Redcat.Xmpp;

namespace Redcat.App.ViewModels
{
    public class AddRosterItemViewModel : MvxViewModel
    {
        private XmppCommunicator communicator;

        public AddRosterItemViewModel(XmppCommunicator communicator)
        {
            this.communicator = communicator;
            AddItemCommand = new MvxCommand(AddItem);
        }

        public string Name { get; set; }

        public JID Jid { get; set; }

        public IMvxCommand AddItemCommand { get; }

        private void AddItem()
        {
            communicator.AddContact(Jid, Name);
            Close(this);
        }
    }
}
