using Redcat.Core.Services;

namespace Redcat.Core
{
    public class CommunicatorServiceProvider : ServiceProviderBase
    {
        public CommunicatorServiceProvider(IKernel kernel)
        {
            AddServiceInstance<IChannelManager>(new ChannelManager(kernel));
        }
    }
}
