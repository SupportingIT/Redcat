using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using System.Net.Sockets;

namespace Redcat.Xmpp.Tests.Integration
{
    [TestFixture]
    public class XmppProtocolExtensionTests
    {
        [Test]
        public void Connect_Initializes_Xmpp_Stream()
        {
            Communicator communicator = new Communicator();
            XmppProtocolExtension extension = new XmppProtocolExtension(CreateTcpSocket);
            communicator.AddExtension(extension);
            communicator.Run();
            ConnectionSettings settings = CreateSettings();

            communicator.Connect(settings);

            Assert.Fail();
        }

        private ISocket CreateTcpSocket()
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            return new SocketAdapter(socket);
        }

        private ConnectionSettings CreateSettings()
        {
            var settings = new ConnectionSettings { Host = "jabber.ru", Domain = "jabber.ru", Port = 5222 };
            settings.Set("ChannelTypeId", "xmpp");
            return settings;
        }
    }
}
