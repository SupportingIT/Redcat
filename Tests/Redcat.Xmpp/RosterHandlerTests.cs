using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class RosterHandlerTests
    {
        [Test]
        public void OnNext_Converts_RequestContacts_Command_To_Roster_Get_Command()
        {
            RosterHandler handler = new RosterHandler();
            ContactCommand command = ContactCommand.Get();
            IObserver<IqStanza> observer = A.Fake<IObserver<IqStanza>>();
            IqStanza iq = null;
            A.CallTo(() => observer.OnNext(A<IqStanza>._)).Invokes(f => {
                iq = f.GetArgument<IqStanza>(0);
            });
            handler.Subscribe(observer);

            handler.OnNext(command);

            Assert.That(iq.IsRosterRequest(), Is.True);
        }

        [Test]
        public void OnNext_Converts_RosterResult_To_()
        {

        }
    }
}
