using Redcat.Core.Communication;
using System.IO;
using System.Net.Sockets;

namespace Redcat.Core.Net
{
    public class TcpChannel : ChannelBase
    {
        private TcpClient tcpClient;

        public TcpChannel(ConnectionSettings settings) : base(settings)
        { }

        protected Stream Stream
        {
            get { return tcpClient?.GetStream(); }
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            tcpClient = new TcpClient();
            tcpClient.Connect(Settings.Host, Settings.Port);
        }

        public void SetSecuredStream()
        {

        }
    }
}
