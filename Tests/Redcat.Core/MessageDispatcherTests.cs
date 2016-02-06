using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture, Ignore]
    public class MessageDispatcherTests
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Dispatch_Throws_Exception_If_No_ActiveConnections()
        {                  
            MessageDispatcher dispatcher = new MessageDispatcher();

            dispatcher.Dispatch("some-string");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Dispatch_Throws_Exception_If_No_Output_Channels_For_Aproriate_Massage()
        {            
            MessageDispatcher dispatcher = new MessageDispatcher();

            dispatcher.Dispatch("some-string");
        }

        [Test]
        public void Dispatch_Sends_Message_Via_Approriate_Output_Connection()
        {            
            MessageDispatcher dispatcher = new MessageDispatcher();
            Guid message = Guid.NewGuid();

            dispatcher.Dispatch(message);
        }

        [Test]
        public void Dispatch_Sends_Message_Via_Default_Connection()
        {
            IOutputChannel<int> defaultChannel = A.Fake<IOutputChannel<int>>();
            IOutputChannel<int> activeChannel = A.Fake<IOutputChannel<int>>();           
            
            MessageDispatcher dispatcher = new MessageDispatcher();
            int message = 8;

            dispatcher.Dispatch(message);

            A.CallTo(() => defaultChannel.Send(message)).MustHaveHappened();
            A.CallTo(() => activeChannel.Send(message)).MustNotHaveHappened();
        }
    }
}
