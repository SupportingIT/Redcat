namespace Redcat.Core
{
    public interface IKernelExtension
    {
        void Attach(Kernel kernel);
        void Detach(Kernel kernel);
    }
}
