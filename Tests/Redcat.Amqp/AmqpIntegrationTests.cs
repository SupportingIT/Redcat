using System;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using System.IO;

namespace Redcat.Amqp.Tests
{
    [TestFixture]
    public class AmqpIntegrationTests
    {
        [Test]
        public void Amqp_Connection_Test()
        {
            ConnectionSettings settings = CreateConnectionSettings();
            TcpChannel channel = new TcpChannel(settings);
            channel.Open();
            Stream stream = channel.GetStream();
            byte[] header = { (byte)'A', (byte)'M', (byte)'Q', (byte)'P', 0, 1, 0, 0 };
            stream.Write(header, 0, 8);
            var response = channel.ReceiveAsync().Result.Array;
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
