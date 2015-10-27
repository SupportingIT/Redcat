namespace Redcat.Core.Services
{
    public interface IMessageDispatcher
    {
        void DispatchIncoming(Message message);
        void DispatchOutgoing(Message message);
    }
}
