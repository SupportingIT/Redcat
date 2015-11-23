using System;
using Redcat.Core.Communication;
using Redcat.Core.Service;

namespace Redcat.Core.Net
{
    public static class NetworkExtensions
    {
        public static void AddNetworkExtension(this Communicator communicator)
        {
            communicator.AddExtension("Redcat.Net", AddExtensions);
        }

        private static void AddExtensions(IServiceCollection services)
        {            
            services.TryAddSingleton<IChannelFactory<IStreamChannel>, TcpChannelFactory>();
        }
    }

    public class TcpChannelFactory : IChannelFactory<IStreamChannel>
    {
        public IStreamChannel CreateChannel(ConnectionSettings settings)
        {
            return new TcpChannel(settings);
        }
    }
}
