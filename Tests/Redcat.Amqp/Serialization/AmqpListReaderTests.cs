using NUnit.Framework;
using Redcat.Amqp.Serialization;
using Redcat.Core;

namespace Redcat.Amqp.Tests.Serialization
{
    [TestFixture]
    public class AmqpListReaderTests
    {
        private ByteBuffer buffer;
        private AmqpListReader reader;

        [SetUp]
        public void SetUp()
        {
            buffer = new ByteBuffer(100);
            reader = new AmqpListReader(buffer);
        }

        byte[] ListTypeCodes = { DataTypeCodes.List32, DataTypeCodes.List8, DataTypeCodes.List0 };

        [Test]
        public void CanRead_Returns_True_For_List_Type_Codes([ValueSource(nameof(ListTypeCodes))]byte code)
        {
            Assert.Fail();
        }
    }
}
