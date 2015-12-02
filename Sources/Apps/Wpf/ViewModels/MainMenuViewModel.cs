using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Input;

namespace Redcat.Communicator.ViewModels
{
    public class MainMenuViewModel
    {
        public MainMenuViewModel()
        {
            ManageAccountsCommand = new DelegateCommand(RiseManageAccounts);
        }

        public InteractionRequest<INotification> ManageAccountsRequest { get; } = new InteractionRequest<INotification>();

        public ICommand ManageAccountsCommand { get; }

        private void RiseManageAccounts()
        {
            Notification notification = new Notification { Title = "Title", Content = "Hello world" };
            ManageAccountsRequest.Raise(notification);
        }
    }
}
