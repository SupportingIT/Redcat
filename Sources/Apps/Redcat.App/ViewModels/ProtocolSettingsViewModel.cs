using Cirrious.MvvmCross.ViewModels;
using Redcat.Core;

namespace Redcat.App.ViewModels
{
    public abstract class ProtocolSettingsViewModel : MvxViewModel
    {
        public abstract ConnectionSettings CreateSettings();
    }
}
