using Redcat.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Amqp.Serialization
{
    public abstract class PerformativeReader<T> : AmqpListReader
    {
        private FieldInitializer[] fieldInitializers;
        
        protected abstract T CreateDefaultPerformative();

        protected abstract IEnumerable<FieldInitializer> GetFieldInitializers();

        public override object Read(ByteBuffer buffer)
        {
            if (fieldInitializers == null) fieldInitializers = GetFieldInitializers().ToArray();
            T performative = CreateDefaultPerformative();

            for (int i = 0; i < fieldInitializers.Length; i++)
            {
                var initializer = fieldInitializers[i];
                initializer.Invoke(performative, buffer);
            }

            return performative;
        }

        public delegate void FieldInitializer(T performative, ByteBuffer buffer);
    }
}
