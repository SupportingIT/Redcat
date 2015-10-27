using System.Collections.Generic;

namespace Redcat.Core.Communication
{
    public interface IChannelManager
    {
        IMessageChannel OpenChannel(ConnectionSettings settings);

        IEnumerable<IMessageChannel> ActiveChannels { get; }
        IMessageChannel DefaultChannel { get; }
    }
}
