using Cirrious.MvvmCross.ViewModels;

namespace Redcat.App.ViewModels
{
    public class ShowProtocolSettingsHint : MvxPresentationHint
    {
        public ShowProtocolSettingsHint(ProtocolSettingsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ProtocolSettingsViewModel ViewModel { get; }
    }
}
