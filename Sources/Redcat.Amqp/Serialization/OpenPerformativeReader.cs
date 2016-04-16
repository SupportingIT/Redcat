using System;
using Redcat.Amqp.Performatives;

namespace Redcat.Amqp.Serialization
{
    public class OpenPerformativeReader : PerformativeReaderBase<Open>
    {
        protected override Open CreateDefault()
        {
            return new Open
            {
                MaxFrameSize = 4294967295,
                MaxChannel = 65535
            };
        }

        protected override Action<Open, AmqpDataReader>[] GetFieldInitializers()
        {
            return new Action<Open, AmqpDataReader>[]
            {
                (p, r) => p.ContainerId = r.ReadString(),
                (p, r) => p.Hostname = r.ReadString(),
                (p, r) => p.MaxFrameSize = r.ReadUInt(),
                (p, r) => p.MaxChannel = r.ReadUShort()
            };
        }
    }
}
