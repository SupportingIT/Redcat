using Redcat.Core;
using System;

namespace Redcat.Amqp.Serialization
{
    public interface IByteBufferReader
    {
        object Read(ByteBuffer buffer);
        T Read<T>(ByteBuffer buffer);
    }
}
