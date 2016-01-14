using System.Threading.Tasks;

namespace Redcat.Core.Channels
{
    public interface IAsyncChannel
    {
        Task OpenAsync();
        Task CloseAsync();
    }

    public interface IAsyncInputChannel<T>
    {
        Task<T> ReceiveAsync();
    }

    public interface IAsyncOutputChannel<T>
    {
        Task SendAsync(T message);
    }
}
