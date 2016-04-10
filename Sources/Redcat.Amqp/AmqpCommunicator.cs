using Redcat.Amqp.Channels;
using Redcat.Core;

namespace Redcat.Amqp
{
    public class AmqpCommunicator : SingleChannelCommunicator<IAmqpChannel>
    {
        private ConnectionModule connectionModule;

        public AmqpCommunicator(IAmqpChannelFactory channelFactory) : base(channelFactory)
        {
            connectionModule = new ConnectionModule(Send);
        }        

        public void Send(AmqpFrame frame)
        {
            Channel.Send(frame);
        }
    }
}
