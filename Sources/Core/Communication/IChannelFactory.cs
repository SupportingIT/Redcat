namespace Redcat.Core.Services
{
    public interface IChannelFactory
    {
        IMessageChannel CreateChannel(ConnectionSettings settings);
    }
}
