using System;
using System.Linq;

namespace Redcat.Core.Communication
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private IChannelManager channelManager;

        public MessageDispatcher(IChannelManager channelManager)
        {
            if (channelManager == null) throw new ArgumentNullException(nameof(channelManager));
            this.channelManager = channelManager;
        }

        public void Dispatch<T>(T message)
        {
            if (channelManager.ActiveChannels.Count() == 0) throw new InvalidOperationException("No active channels");            
            var channel = SelectChannel(channelManager, message);
            if (channel == null) throw new InvalidOperationException("No approriate output channels");
            SendMessage(channel, message);
        }

        private IOutputChannel<T> SelectChannel<T>(IChannelManager channelManager, T message)
        {
            if (channelManager.DefaultChannel is IOutputChannel<T>) return (IOutputChannel<T>)channelManager.DefaultChannel;
            var channels = channelManager.ActiveChannels.OfType<IOutputChannel<T>>();
            return channels.FirstOrDefault();
        }

        private void SendMessage<T>(IOutputChannel<T> channel, T message)
        {
            channel.Send(message);
        }
    }
}
