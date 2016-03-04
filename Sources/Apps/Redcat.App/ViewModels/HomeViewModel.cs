using MvvmCross.Core.ViewModels;

namespace Redcat.App.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public HomeViewModel()
        {            
            ShowViewCommand = new MvxCommand(ShowView);
        }        

        public IMvxCommand ShowViewCommand { get; }

        private void ShowView()
        {
            ShowViewModel<XmppCommunicatorViewModel>();
        }
    }    
}
