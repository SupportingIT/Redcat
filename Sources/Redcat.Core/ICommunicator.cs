namespace Redcat.Core
{
    public interface ICommunicator
    {
        void Connect(ConnectionSettings settings);
        void Disconnect();
    }
}
