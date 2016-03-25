using Redcat.Amqp.Channels;
using Redcat.Core;

namespace Redcat.Amqp
{
    public class AmqpCommunicator : SingleChannelCommunicator<IAmqpChannel>
    {
        private ConnectionModule connectionModule;

        public AmqpCommunicator(IAmqpChannelFactory channelFactory) : base(channelFactory)
        {            
        }

        protected override void OnChannelCreated(IAmqpChannel channel)
        {
            base.OnChannelCreated(channel);
            connectionModule = new ConnectionModule(Send);
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            connectionModule.OpenConnection();
        }

        public void Send(AmqpFrame frame)
        {
            Channel.Send(frame);
        }
    }
}
