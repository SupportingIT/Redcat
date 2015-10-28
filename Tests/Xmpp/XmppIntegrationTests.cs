using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using System;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppIntegrationTests
    {
        [Test]
        public void Client_Connection()
        {
            Communicator communicator = new Communicator();
            communicator.AddXmppExtension();
            communicator.Run();

            ConnectionSettings settings = CreateConnectionSettings();
            communicator.Connect(settings);
        }

        private ISocket CreateSocket()
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            return new SocketAdapter(socket);
        }

        private Stream CreateTlsStream(Stream stream)
        {
            SslStream sslStream = new SslStream(stream);
            sslStream.AuthenticateAsClient("localhost");
            throw new NotImplementedException();
        }

        private ConnectionSettings CreateConnectionSettings()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ChannelType = "xmpp";
            settings.Domain = ConfigurationManager.AppSettings["Domain"];
            settings.Host = ConfigurationManager.AppSettings["Host"];
            settings.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            return settings;
        }        
    }
}
