using Redcat.App.ViewModels;
using System.Collections.Generic;

namespace Redcat.App.Services
{
    public interface IProtocolService
    {
        IEnumerable<string> GetProtocolsName();
        ProtocolSettingsViewModel GetViewModelForNewSettings(string protocolName);
        ProtocolSettingsViewModel GetViewModelForEditSettings(string protocolName);
    }
}
