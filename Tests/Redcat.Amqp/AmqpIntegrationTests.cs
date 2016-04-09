using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Amqp.Channels;

namespace Redcat.Amqp.Tests
{
    [Category("Integration")]
    [TestFixture]
    public class AmqpIntegrationTests
    {
        [Test, Ignore]
        public void Amqp_Connection_Test()
        {
            ConnectionSettings settings = CreateConnectionSettings();
            var communicator = CreateCommunicator();
            communicator.Connect(settings);
            communicator.Disconnect();
        }

        private AmqpCommunicator CreateCommunicator()
        {
            return null;
        }

        private ConnectionSettings CreateConnectionSettings()
        {
            return new ConnectionSettings
            {
                Host = "localhost",
                Port = 5672
            };
        }
    }
}
