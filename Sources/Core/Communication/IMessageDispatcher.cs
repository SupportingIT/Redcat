namespace Redcat.Core.Communication
{
    public interface IMessageDispatcher
    {
        void Dispatch<T>(T message);
    }
}
