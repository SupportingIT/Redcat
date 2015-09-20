using System.Threading.Tasks;

namespace Redcat.Core.Net
{
    public interface ISocket
    {
        void Connect(ConnectionSettings settings);
        void Disconnect();

        int Available { get; } 
        bool IsConnected { get; }

        int Receive(byte[] buffer, int offset, int size);
        int Send(byte[] buffer, int offset, int size);
    }

    public interface IAsyncSocket
    {
        Task ConnectAsync(ConnectionSettings settings);
        Task DisconnectAsync();

        int Available { get; }
        bool IsConnected { get; }
    }
}
