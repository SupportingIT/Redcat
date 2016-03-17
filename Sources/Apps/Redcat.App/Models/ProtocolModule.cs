using System;

namespace Redcat.App.Models
{
    public class ProtocolModule
    {
        public ProtocolModule(string protocolName, Type homeViewModel)
        {
            ProtocolName = protocolName;
            HomeViewModel = homeViewModel;
        }

        public string ProtocolName { get; }

        public Type HomeViewModel { get; }
    }
}
