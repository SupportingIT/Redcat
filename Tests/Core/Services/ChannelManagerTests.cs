using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Services;
using System;
using System.Linq;

namespace Redcat.Core.Tests.Services
{
    [TestFixture]
    public class ChannelManagerTests
    {
        [Test]
        public void OpenChannel_Uses_Correct_Factory_To_Create_Channel()
        {
            var factory1 = new TestChannelFactory();
            var factory2 = new Test2ChannelFactory();
            ChannelManager manager = CreateManager(factory1, factory2);
            ConnectionSettings settings = CreateSettings("test2");

            manager.OpenChannel(settings);

            Assert.That(factory2.ChannelCreated, Is.True);
        }

        [Test]
        public void OpenChannel_Opens_Created_Channel()
        {
            var factory = new TestChannelFactory();
            ChannelManager manager = CreateManager(factory);
            ConnectionSettings settings = CreateSettings("test");

            var channel = manager.OpenChannel(settings);

            A.CallTo(() => channel.Open()).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenChannel_Throws_Exception_If_No_Channels_For_Specified_ChannelTypeId()
        {
            var factory = new Test2ChannelFactory();            
            ChannelManager manager = CreateManager(factory);
            ConnectionSettings settings = CreateSettings("test");

            manager.OpenChannel(settings);
        }

        [Test]
        public void OpenChannel_Adds_Created_Channel_To_Active_Channels()
        {
            ChannelManager manager = CreateManager(new TestChannelFactory());
            ConnectionSettings settings = CreateSettings("test");

            var channel1 = manager.OpenChannel(settings);
            var channel2 = manager.OpenChannel(settings);

            Assert.That(manager.ActiveChannels.Count(), Is.EqualTo(2));
            CollectionAssert.AreEquivalent(new[] { channel1, channel2 }, manager.ActiveChannels);
        }

        [Test]
        public void OpenChannel_Sets_DefaultChannel()
        {
            ChannelManager manager = CreateManager(new TestChannelFactory());
            ConnectionSettings settings = CreateSettings("test");

            Assert.That(manager.DefaultChannel, Is.Null);
            manager.OpenChannel(settings);
            Assert.That(manager.DefaultChannel, Is.Not.Null);
        }

        [Test]
        public void Closing_Channel_Removes_From_ActiveChannels_And_DefaultChannel()
        {
            ChannelManager manager = CreateManager(new TestChannelFactory());
            ConnectionSettings settings = CreateSettings("test");
            var channel = manager.OpenChannel(settings);

            channel.StateChanged += Raise.With(new StateChangedEventArgs(ChannelState.Close));

            Assert.That(manager.ActiveChannels.Count(), Is.EqualTo(0));
            Assert.That(manager.DefaultChannel, Is.Null);
        }

        private ChannelManager CreateManager(params IChannelFactory[] factories)
        {            
            return new ChannelManager();
        }        

        private ConnectionSettings CreateSettings(string channelTypeId)
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.Set("ChannelTypeId", channelTypeId);
            return settings;
        }

        class TestChannelFactory : IChannelFactory
        {
            public IMessageChannel CreateChannel(ConnectionSettings settings)
            {
                ChannelCreated = true;
                return A.Fake<IMessageChannel>();
            }

            public bool ChannelCreated { get; private set; }
        }

        class Test2ChannelFactory : TestChannelFactory
        { }
    }
}
