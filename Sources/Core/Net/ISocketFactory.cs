namespace Redcat.Core.Net
{
    public interface ISocketFactory
    {
        ISocket CreateSocket(ConnectionSettings settings);
    }
}
