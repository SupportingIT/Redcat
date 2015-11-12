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
            services.TryAddSingleton<ISocketFactory, SocketFactory>();
            services.TryAddSingleton<INetworkStreamFactory, NetworkStreamFactory>();
        }
    }
}
