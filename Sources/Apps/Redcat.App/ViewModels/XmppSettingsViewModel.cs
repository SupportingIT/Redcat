using System;
using Redcat.Core;

namespace Redcat.App.ViewModels
{
    public class XmppSettingsViewModel : ProtocolSettingsViewModel
    {
        public string Username
        {
            get; set;
        }

        public string Domain
        {
            get; set;
        }

        public string Resource
        {
            get; set;
        }

        public override ConnectionSettings CreateSettings()
        {
            throw new NotImplementedException();
        }
    }
}
