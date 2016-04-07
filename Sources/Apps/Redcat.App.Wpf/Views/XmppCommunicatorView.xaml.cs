using MvvmCross.Wpf.Views;
using Redcat.App.ViewModels;

namespace Redcat.App.Wpf.Views
{
    public partial class XmppCommunicatorView : MvxWpfView
    {
        public XmppCommunicatorView()
        {
            InitializeComponent();
        }

        public new XmppCommunicatorViewModel ViewModel => base.ViewModel as XmppCommunicatorViewModel;        
    }
}
