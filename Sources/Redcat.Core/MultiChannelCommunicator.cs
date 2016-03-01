using Redcat.Core.Channels;
using System.Collections.Generic;
namespace Redcat.Core
{
    public class MultiChannelCommunicator<T> : CommunicatorBase<T> where T : IChannel
    {
        private ICollection<IChannel> openChannels;

        public MultiChannelCommunicator(IChannelFactory<T> channelFactory) : base(channelFactory)
        {
            openChannels = new List<IChannel>();
        }

        protected IEnumerable<IChannel> OpenChannels => openChannels;

        protected override void OnChannelCreated(T channel)
        {
            base.OnChannelCreated(channel);
            channel.Open();
            openChannels.Add(channel);
        }
    }
}
