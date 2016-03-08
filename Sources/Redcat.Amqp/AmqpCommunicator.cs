using Redcat.Amqp.Channels;
using Redcat.Core;

namespace Redcat.Amqp
{
    public class AmqpCommunicator : SingleChannelCommunicator<IAmqpChannel>
    {
        public AmqpCommunicator(IAmqpChannelFactory channelFactory) : base(channelFactory)
        {
        }

        public void Send(Frame frame)
        {
            Channel.Send(frame);
        }
    }
}
