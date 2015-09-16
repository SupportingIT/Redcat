namespace Redcat.Core
{
    public interface IKernelExtension
    {
        void Attach(IKernel kernel);
        void Detach(IKernel kernel);
    }
}
