using Redcat.Core;
using Redcat.Core.Channels;

namespace Redcat.Amqp.Channels
{
    public class AmqpChannelFactory : IAmqpChannelFactory
    {
        private IReactiveStreamChannelFactory streamChannelFactory;

        public AmqpChannelFactory(IReactiveStreamChannelFactory streamChannelFactory)
        {
            this.streamChannelFactory = streamChannelFactory;
        }

        public IAmqpChannel CreateChannel(ConnectionSettings settings)
        {
            IReactiveStreamChannel streamChannel = streamChannelFactory.CreateChannel(settings);
            return new AmqpChannel(streamChannel, settings);
        }
    }
}
