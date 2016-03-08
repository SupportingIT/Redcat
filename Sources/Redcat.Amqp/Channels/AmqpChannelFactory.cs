using Redcat.Core;
using Redcat.Core.Channels;

namespace Redcat.Amqp.Channels
{
    public class AmqpChannelFactory : IAmqpChannelFactory
    {
        private const int BufferSize = 10000;
        private IStreamChannelFactory streamChannelFactory;

        public AmqpChannelFactory(IStreamChannelFactory streamChannelFactory)
        {
            this.streamChannelFactory = streamChannelFactory;
        }

        public IAmqpChannel CreateChannel(ConnectionSettings settings)
        {
            IStreamChannel streamChannel = streamChannelFactory.CreateChannel(settings);
            return new AmqpChannel(streamChannel, settings, BufferSize);
        }
    }
}
