namespace Redcat.Core.Communication
{
    public interface IChannelFactory
    {
        IChannel CreateChannel(ConnectionSettings settings);
    }

    public interface IChannelFactory<T> where T : IChannel
    {
        T CreateChannel(ConnectionSettings settings);
    }
}
