using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Services;
using FakeItEasy;
using Redcat.Core.Communication;
using System.IO;

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
            IXmppStream xmppStream = A.Fake<IXmppStream>();
            IStreamChannel streamChannel = CreateStreamChannel();

            XmppChannel channel = new XmppChannel(initializer, streamChannel, settings);

            channel.Open();

            A.CallTo(() => initializer.Init(A<IXmppStream>._)).MustHaveHappened();
        }

        private IStreamChannel CreateStreamChannel()
        {
            IStreamChannel streamChannel = A.Fake<IStreamChannel>();
            Stream stream = A.Fake<Stream>();

            A.CallTo(() => stream.CanRead).Returns(true);
            A.CallTo(() => stream.CanWrite).Returns(true);
            A.CallTo(() => streamChannel.GetStream()).Returns(stream);

            return streamChannel;
        }
    }
}
