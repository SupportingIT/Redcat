﻿using System;
using System.IO;

namespace Redcat.Core.Serializaton
{
    public interface IDeserializer<T>
    {
        T Deserialize(Stream stream);
    }

    public interface IReactiveDeserializer<T>
    {
        void Read(ArraySegment<byte> binaryData);
        event Action<T> Deserialized;
    }
}
