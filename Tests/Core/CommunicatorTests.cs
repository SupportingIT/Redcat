using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System.Linq;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommunicatorTests
    {
        [Test]
        public void Connect_Crates_Connection_With_Given_Name()
        {
            IChannelFactory factory = A.Fake<IChannelFactory>();
            Communicator communicator = new Communicator(factory);
            string connectionName = "ConnName";
            ConnectionSettings settings = new ConnectionSettings { ConnectionName = connectionName };
            communicator.Connect(settings);

            Connection connection = communicator.ActiveConnections.Single();

            Assert.That(connection.Name, Is.EqualTo(connectionName));
        }
    }
}
