namespace Redcat.Core
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}
