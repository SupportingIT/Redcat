using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class RosterHandlerTests
    {
        private RosterHandler handler;
        private IObserver<IqStanza> observer;
        private IqStanza iq;        

        [SetUp]
        public void SetUp()
        {
            handler = new RosterHandler();
            observer = A.Fake<IObserver<IqStanza>>();
            A.CallTo(() => observer.OnNext(A<IqStanza>._)).Invokes(f => {
                iq = f.GetArgument<IqStanza>(0);
            });
            handler.Subscribe(observer);
        }

        [Test]
        public void OnNext_Converts_GetContacts_Command_To_Roster_Get_Iq()
        {
            ContactCommand command = ContactCommand.Get();

            handler.OnNext(command);

            Assert.That(iq.IsRosterRequest(), Is.True);
        }

        [Test]
        public void OnNext_Converts_AddContact_Command_To_Add_Roster_Iq()
        {
            RosterItem contact = new RosterItem("user@domain.com");
            ContactCommand command = ContactCommand.Add(contact);

            handler.OnNext(command);

            var item = iq.GetRosterItems().Single();
            Assert.That(iq.IsSet(), Is.True);
            Assert.That(item.GetAttributeValue<JID>("jid"), Is.EqualTo(contact.Jid));
        }

        [Test]
        public void OnNext_Converts_RemoveContact_Command_To_Remove_Roster_Iq()
        {
            RosterItem contact = new RosterItem("user@domain.com");
            ContactCommand command = ContactCommand.Remove(contact);

            handler.OnNext(command);

            var item = iq.GetRosterItems().Single();
            Assert.That(iq.IsSet(), Is.True);
            Assert.That(item.GetAttributeValue<JID>("jid"), Is.EqualTo(contact.Jid));
            Assert.That(item.GetAttributeValue<string>("subscription"), Is.EqualTo("remove"));
        }

        [Test]
        public void OnNext_Converts_RosterResult_To_ListContacts_Command()
        {
            Contact[] contacts = Enumerable.Range(0, 3).Select(i => new RosterItem($"user{i}@domain.com")).ToArray();
            IObserver<ContactCommand> commandObserver = A.Fake<IObserver<ContactCommand>>();
            ContactCommand receivedCommand = null;
            A.CallTo(() => commandObserver.OnNext(A<ContactCommand>._)).Invokes(c => 
            {
                receivedCommand = c.GetArgument<ContactCommand>(0);
            });
            handler.Subscribe(commandObserver);

            handler.OnNext(Roster.Result(contacts));

            Assert.That(receivedCommand.IsList(), Is.True);
            Assert.That(receivedCommand.Contacts.Count(), Is.EqualTo(3));
        }
    }
}
