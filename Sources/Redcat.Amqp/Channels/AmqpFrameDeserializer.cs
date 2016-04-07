using System;
using Redcat.Core.Channels;

namespace Redcat.Amqp.Channels
{
    public class AmqpFrameDeserializer : ReactiveDeserializerBase<AmqpFrame>
    {
        const int DefaultBufferSize = 1024;

        public AmqpFrameDeserializer() : base(DefaultBufferSize)
        { }

        protected override void OnBufferUpdated()
        {
            throw new NotImplementedException();
        }
    }
}
