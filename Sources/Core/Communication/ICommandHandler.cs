namespace Redcat.Core.Services
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}
