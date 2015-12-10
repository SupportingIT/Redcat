using System;
using System.Collections.Generic;

namespace Redcat.App.Services
{
    public interface IProtocolInfoProvider
    {
        IEnumerable<string> GetProtocolsName();
        Type GetViewModelTypeForNewSettings(string protocolName);
        Type GetViewModelTypeForEditSettings(string protocolName);
    }
}
