namespace Redcat.Core
{
    public interface IHandler<T>
    {
        void Handle(T message);
    }
}
