using FakeItEasy;
using NUnit.Framework;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class ContactControllerTests
    {
        private ContactController controller;
        private ContactCommand command;
        private Contact contact;
        private IObserver<ContactCommand> observer;

        [SetUp]
        public void SetUp()
        {
            controller = new ContactController();
            command = null;
            contact = A.Fake<Contact>();
            observer = CreateObserver(controller);
        }

        [Test]
        public void ListCommand_Fill_Contact_List()
        {
            ContactController controller = new ContactController();
            var contacts = A.CollectionOfFake<Contact>(3);
            var command = ContactCommand.List(contacts);

            controller.OnNext(command);

            CollectionAssert.AreEquivalent(contacts, controller.Contacts);
        }

        [Test]
        public void Add_Sends_AddCommand_And_Adds_Contact_To_List()
        {
            controller.Add(contact);

            CollectionAssert.Contains(controller.Contacts, contact);
            Assert.That(command.IsAdd(), Is.True);
            Assert.That(command.Contact, Is.SameAs(contact));
        }

        [Test]
        public void Remove_Sends_RemoveCommand_And_Removes_Contact_From_List()
        {            
            controller = new ContactController(contact);
            observer = CreateObserver(controller);

            controller.Remove(contact);

            CollectionAssert.DoesNotContain(controller.Contacts, contact);
            Assert.That(command.IsRemove(), Is.True);
            Assert.That(command.Contact, Is.SameAs(contact));
        }

        [Test]
        public void Update_Sends_UpdateCommand()
        {
            var contacts = A.CollectionOfFake<Contact>(4);
            controller.OnNext(ContactCommand.List(contacts));            

            controller.Update();

            Assert.That(command.IsUpdate(), Is.True);
            CollectionAssert.AreEquivalent(contacts, command.Contacts);
        }

        [Test]
        public void AddCommand_Adds_Contact()
        {
            command = ContactCommand.Add(contact);

            controller.OnNext(command);

            CollectionAssert.Contains(controller.Contacts, contact);
        }

        private IObserver<ContactCommand> CreateObserver(ContactController controller)
        {
            var observer = A.Fake<IObserver<ContactCommand>>();
            controller.Subscribe(observer);
            A.CallTo(() => observer.OnNext(A<ContactCommand>._)).Invokes(c => {
                command = c.GetArgument<ContactCommand>(0);
            });
            return observer;
        }
    }
}
