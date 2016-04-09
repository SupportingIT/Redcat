namespace Redcat.Core.Channels
{
    public interface IChannelFactory
    {
        IChannel CreateChannel(ConnectionSettings settings);
    }

    public interface IChannelFactory<T> where T : IChannel
    {
        T CreateChannel(ConnectionSettings settings);
    }

    public interface IStreamChannelFactory : IChannelFactory<IStreamChannel>
    { }

    public interface IReactiveStreamChannelFactory : IChannelFactory<IReactiveStreamChannel>
    { }
}
