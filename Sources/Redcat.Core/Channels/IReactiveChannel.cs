using System;

namespace Redcat.Core.Channels
{
    public interface IReactiveInputChannel<T> : IChannel
    {
        event EventHandler<ChannelMessageEventArgs<T>> MessageReceived;
    }
}
