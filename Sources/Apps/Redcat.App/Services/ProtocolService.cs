using System;
using System.Collections.Generic;
using Redcat.App.ViewModels;

namespace Redcat.App.Services
{
    public class ProtocolService : IProtocolService
    {
        public IEnumerable<string> GetProtocolsName()
        {
            return new[] { "XMPP" };
        }

        public ProtocolSettingsViewModel GetViewModelForEditSettings(string protocolName)
        {
            throw new NotImplementedException();
        }

        public ProtocolSettingsViewModel GetViewModelForNewSettings(string protocolName)
        {
            return new XmppSettingsViewModel();
        }

        public void AddProtocol(string protocolName)
        {

        }
    }    
}
