using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Net;
using System;
using System.IO;

namespace Redcat.Core.Tests.Net
{
    [TestFixture]
    public class NetworkChannelTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Throws_Exception_If_NetworkStreamFactory_Is_Null()
        {
            NetworkChannel channel = new NetworkChannelStub(null, new ConnectionSettings());
        }

        [Test]
        public void Open_Creates_New_Stream()
        {
            INetworkStreamFactory factory = A.Fake<INetworkStreamFactory>();
            ConnectionSettings settings = new ConnectionSettings();
            NetworkChannel channel = CreateChannel(factory, settings);

            channel.Open();

            A.CallTo(() => factory.CreateStream(settings)).MustHaveHappened();
        }

        [Test]
        public void Close_Disposes_Created_Stream()
        {
            INetworkStreamFactory factory = A.Fake<INetworkStreamFactory>();
            Stream stream = A.Fake<Stream>();
            A.CallTo(() => factory.CreateStream(A<ConnectionSettings>._)).Returns(stream);
            NetworkChannel channel = CreateChannel(factory);
            channel.Open();

            channel.Close();

            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Send_Throws_Exception_If_Buffer_Is_Null()
        {
            NetworkChannel channel = CreateChannel(A.Fake<INetworkStreamFactory>());

            channel.Send(null, 0, 1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Send_Throws_Exception_If_Channel_Was_Not_Opened()
        {
            NetworkChannel channel = CreateChannel(A.Fake<INetworkStreamFactory>());

            channel.Send(new byte[] { 0x01, 0x02, 0x03 }, 0, 3);
        }

        [Test]
        public void Send_Uses_Created_Stream()
        {
            INetworkStreamFactory factory = A.Fake<INetworkStreamFactory>();
            Stream stream = A.Fake<Stream>();
            A.CallTo(() => factory.CreateStream(A<ConnectionSettings>._)).Returns(stream);
            NetworkChannel channel = CreateChannel(factory);
            byte[] buffer = new byte[] { 1, 2, 3 };

            channel.Open();
            channel.Send(buffer, 0, buffer.Length);

            A.CallTo(() => stream.Write(buffer, 0, buffer.Length)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetSecuredStream_Throws_Exception_If_Channel_Was_Not_Opened()
        {
            NetworkChannel channel = CreateChannel(A.Fake<INetworkStreamFactory>());

            channel.SetSecuredStream();
        }

        [Test]
        public void SetSecuredStream_Creates_Stream_Using_Factory()
        {
            INetworkStreamFactory factory = A.Fake<INetworkStreamFactory>();
            Stream stream = A.Fake<Stream>();
            A.CallTo(() => factory.CreateStream(A<ConnectionSettings>._)).Returns(stream);
            ConnectionSettings settings = new ConnectionSettings();
            NetworkChannel channel = CreateChannel(factory, settings);

            channel.Open();
            channel.SetSecuredStream();

            A.CallTo(() => factory.CreateSecuredStream(stream, settings)).MustHaveHappened();
        }

        [Test]
        public void SetSecuredStream_Send_Data_Via_Secured_Stream()
        {
            INetworkStreamFactory factory = A.Fake<INetworkStreamFactory>();
            Stream securedStream = A.Fake<Stream>();
            A.CallTo(() => factory.CreateSecuredStream(A<Stream>._, A<ConnectionSettings>._)).Returns(securedStream);
            NetworkChannel channel = CreateChannel(factory);
            byte[] buffer = new byte[] { 1, 2, 3, 4 };
            channel.Open();
            channel.SetSecuredStream();

            channel.Send(buffer, 0, buffer.Length);

            A.CallTo(() => securedStream.Write(buffer, 0, buffer.Length)).MustHaveHappened();
        }

        private NetworkChannel CreateChannel(INetworkStreamFactory factory, ConnectionSettings settings = null)
        {
            if (settings == null) settings = new ConnectionSettings();
            return A.Fake<NetworkChannel>(b =>
            {
                b.WithArgumentsForConstructor(new object[] { factory, settings });
                b.CallsBaseMethods();
            });
        }
    }

    internal class NetworkChannelStub : NetworkChannel
    {
        internal NetworkChannelStub(INetworkStreamFactory factory, ConnectionSettings settings) : base(factory, settings)
        { }

        public override Message Receive()
        {
            throw new NotImplementedException();
        }

        public override void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
