namespace Redcat.Core.Communication
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}
