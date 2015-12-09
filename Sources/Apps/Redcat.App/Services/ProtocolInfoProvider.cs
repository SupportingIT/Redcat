using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.App.Services
{
    public class ProtocolInfoProvider : IProtocolInfoProvider
    {
        private IDictionary<string, IProtocolUiService> protocols = new Dictionary<string, IProtocolUiService>();

        public IEnumerable<string> GetProtocolsName()
        {
            return Enumerable.Range(0, 10).Select(i => "Protocol " + i);
        }

        public IProtocolUiService GetUiService(string protocolName)
        {
            throw new NotImplementedException();
        }

        public void RegisterProtocolProvider(string protocolName, IProtocolUiService uiService)
        {
            protocols.Add(protocolName, uiService);
        }
    }
}
