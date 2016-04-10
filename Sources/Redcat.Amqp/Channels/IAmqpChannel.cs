using Redcat.Core.Channels;

namespace Redcat.Amqp.Channels
{
    public interface IAmqpChannel : IReactiveInputChannel<AmqpFrame>, IOutputChannel<AmqpFrame>, IOutputChannel<ProtocolHeader>
    {
    }
}
