using System;
using System.Collections.Generic;

namespace Redcat.App.Services
{
    public class ProtocolInfoProvider : IProtocolInfoProvider
    {
        private IDictionary<string, Tuple<Type, Type>> protocols = new Dictionary<string, Tuple<Type, Type>>();

        public IEnumerable<string> GetProtocolsName()
        {
            return protocols.Keys;
        }

        public Type GetViewModelTypeForNewSettings(string protocolName)
        {
            return protocols[protocolName].Item1;
        }

        public Type GetViewModelTypeForEditSettings(string protocolName)
        {
            return protocols[protocolName].Item2;
        }

        public void RegisterProtocol(string protocolName, Type newSettingsVmType, Type editSettingsVmType)
        {            
            protocols[protocolName] = new Tuple<Type, Type>(newSettingsVmType, editSettingsVmType);
        }
    }
}
