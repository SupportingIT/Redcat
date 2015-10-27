namespace Redcat.Core.Communication
{
    public interface IMessageDispatcher
    {
        void DispatchIncoming(Message message);
        void DispatchOutgoing(Message message);
    }
}
