using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private IOutputChannelProvider channelProvider;

        public MessageDispatcher(IOutputChannelProvider channelProvider)
        {
            this.channelProvider = channelProvider;
        }

        public void Dispatch<T>(T message) where T : class
        {
            if (message == null) throw new NullReferenceException(nameof(message));
            IOutputChannel<T> channel = channelProvider.GetChannel(message);
            if (channel == null) throw new InvalidOperationException();
            channel.Send(message);
        }        
    }

    public interface IOutputChannelProvider
    {
        IOutputChannel<T> GetChannel<T>(T message);
    }
}
