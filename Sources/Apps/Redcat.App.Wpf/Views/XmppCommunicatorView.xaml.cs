using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MvvmCross.Wpf.Views;
using Redcat.Xmpp;

namespace Redcat.App.Wpf.Views
{
    public partial class XmppCommunicatorView : MvxWpfView
    {
        private CustomDialog addRosterItemDialog;

        public XmppCommunicatorView()
        {
            InitializeComponent();
            addRosterItemDialog = (CustomDialog)Resources["AddRosterItemDialog"];
        }

        private void AddItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            addRosterItemDialog.DataContext = DataContext;
            addRosterItemDialog.Resources["NewRosterItem"] = new RosterItem();
            addRosterItemDialog.ShowModalDialogExternally();
        }

        private void CloseDialogButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            addRosterItemDialog.RequestCloseAsync();
        }
    }
}
