namespace Redcat.Core
{
    public interface ICommunicator
    {
        void Connect(ConnectionSettings settings);
        void Disconnect();
        void Send<T>(T message) where T : class;
    }
}
