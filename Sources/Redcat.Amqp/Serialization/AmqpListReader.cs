using Redcat.Core;
using System;

namespace Redcat.Amqp.Serialization
{
    public class AmqpListReader : AmqpDataReader2, IDisposable
    {
        private uint size;
        private uint count;
        private bool isInitialized;

        public AmqpListReader(ByteBuffer buffer) : base(buffer)
        { }

        public override bool CanRead()
        {
            throw new NotImplementedException();
        }

        public override object Read()
        {
            return base.Read();
        }

        public override T Read<T>()
        {            
            return base.Read<T>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
