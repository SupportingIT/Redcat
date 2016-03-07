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
            RequestSubscription = true;
        }

        public string Name { get; set; }

        public JID Jid { get; set; }

        public bool RequestSubscription { get; set; }

        public IMvxCommand AddItemCommand { get; }

        private void AddItem()
        {
            communicator.AddRosterItem(Jid, Name);
            Close(this);
        }
    }
}
