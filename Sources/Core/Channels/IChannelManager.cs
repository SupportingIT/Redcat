using System.Collections.Generic;

namespace Redcat.Core.Channels
{
    public interface IChannelManager
    {
        IChannel OpenChannel(ConnectionSettings settings);
        IEnumerable<IChannel> ActiveChannels { get; }
        IChannel DefaultChannel { get; }
    }
}
