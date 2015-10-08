using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Services;
using FakeItEasy;
using Redcat.Core.Net;

namespace Redcat.Xmpp.Tests.Services
{
    [TestFixture]
    public class XmppChannelTests
    {
        [Test]
        public void Open_Initializes_Stream()
        {
            ConnectionSettings settings = new ConnectionSettings { Domain = "redcat" };
            IStreamInitializer initializer = A.Fake<IStreamInitializer>();
            IXmppStream stream = A.Fake<IXmppStream>();
            TestXmppChannel channel = new TestXmppChannel(A.Fake<ISocket>(), settings) { Initializer = initializer, Stream = stream };

            channel.Open();

            A.CallTo(() => initializer.Start(stream)).MustHaveHappened();
        }

        internal class TestXmppChannel : XmppChannel
        {
            private IXmppStream stream;
            
            public TestXmppChannel(ISocket socket, ConnectionSettings settings) : base(socket, settings)
            {
            }

            public IStreamInitializer Initializer { get; set; }

            public IXmppStream Stream { get; set; }

            protected override IStreamInitializer CreateStreamInitializer(ConnectionSettings settings)
            {
                return Initializer ?? base.CreateStreamInitializer(settings);
            }

            protected override IXmppStream OpenXmppStream()
            {
                return Stream ?? base.OpenXmppStream();
            }
        }
    }
}
