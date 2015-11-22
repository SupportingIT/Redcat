namespace Redcat.Core.Communication
{
    public interface IChannelFactory
    {
        IChannel CreateChannel(ConnectionSettings settings);
    }
}
