using System;

namespace Redcat.Amqp.Serialization
{
    public interface IDataReader
    {
        object Read();
        T Read<T>();
    }
}
