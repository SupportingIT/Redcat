using System;
using Redcat.Amqp.Performatives;
using System.Collections.Generic;
using Redcat.Core;

namespace Redcat.Amqp.Serialization
{
    public class OpenPerformativeReader : PerformativeReader<Open>
    {
        protected override Open CreateDefaultPerformative()
        {
            return new Open
            {
                MaxFrameSize = 4294967295,
                MaxChannel = 65535
            };
        }

        protected override IEnumerable<FieldInitializer> GetFieldInitializers()
        {
            yield return (p, b) => p.ContainerId = Read<string>(b);
            yield return (p, b) => p.Hostname = Read<string>(b);
            yield return (p, b) => p.MaxFrameSize = Read<uint>(b);
            yield return (p, b) => p.MaxChannel = Read<ushort>(b);
            
        }

        public override object Read(ByteBuffer buffer)
        {
            var descriptor = base.Read<Descriptor>(buffer);
            return base.Read(buffer);
        }
    }
}
