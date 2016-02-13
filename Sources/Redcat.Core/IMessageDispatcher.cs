namespace Redcat.Core
{
    public interface IMessageDispatcher
    {
        void Dispatch<T>(T message) where T : class;
    }
}
