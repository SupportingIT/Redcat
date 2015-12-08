using System.Collections.Generic;

namespace Redcat.Communicator.Services
{
    public interface IProtocolInfoProvider
    {
        IEnumerable<string> GetProtocolsName();
        IProtocolUiService GetUiService(string protocolName);
        void RegisterProtocolProvider(string protocolName, IProtocolUiService uiService);
    }
}
