using Redcat.Core.Channels;

namespace Redcat.Amqp.Channels
{
    public interface IAmqpChannel : IOutputChannel<AmqpFrame>
    {
    }
}
