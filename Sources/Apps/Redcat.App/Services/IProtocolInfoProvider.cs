using System.Collections.Generic;

namespace Redcat.App.Services
{
    public interface IProtocolInfoProvider
    {
        IEnumerable<string> GetProtocolsName();
        IProtocolUiService GetUiService(string protocolName);
        void RegisterProtocolProvider(string protocolName, IProtocolUiService uiService);
    }
}
