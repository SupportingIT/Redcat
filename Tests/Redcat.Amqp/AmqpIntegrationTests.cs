using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Amqp.Channels;
using System.Threading;

namespace Redcat.Amqp.Tests
{
    [Category("Integration")]
    [TestFixture]
    public class AmqpIntegrationTests
    {
        [Test]
        public void Amqp_Connection_Test()
        {
            ConnectionSettings settings = CreateConnectionSettings();
            ManualResetEventSlim resetEvent = new ManualResetEventSlim(false);
            var communicator = CreateCommunicator();
            communicator.Connect(settings);
            resetEvent.Wait();
            communicator.Disconnect();
        }

        private AmqpCommunicator CreateCommunicator()
        {
            TcpChannelFactory streamFactory = new TcpChannelFactory() { AcceptAllCertificates = true };
            AmqpChannelFactory factory = new AmqpChannelFactory(streamFactory);
            return new AmqpCommunicator(factory);
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
