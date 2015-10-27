namespace Redcat.Core.Communication
{
    public interface IChannelFactory
    {
        IMessageChannel CreateChannel(ConnectionSettings settings);
    }
}
