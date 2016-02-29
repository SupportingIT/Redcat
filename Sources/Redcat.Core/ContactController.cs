using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class ContactController : IObserver<ContactCommand>, IObservable<ContactCommand>
    {
        private List<Contact> contacts;
        private List<IObserver<ContactCommand>> observers;        

        public ContactController(params Contact[] contacts)
        {
            this.contacts = new List<Contact>(contacts);
            observers = new List<IObserver<ContactCommand>>();
        }

        public IEnumerable<Contact> Contacts => contacts;

        public void Add(Contact contact)
        {
            observers.OnNext(ContactCommand.Add(contact));
            contacts.Add(contact);
        }

        public void Remove(Contact contact)
        {
            observers.OnNext(ContactCommand.Remove(contact));
            contacts.Remove(contact);
        }

        public void RequestContacts() => observers.OnNext(ContactCommand.Get());

        public void Update()
        {
            observers.OnNext(ContactCommand.Update(contacts));
        }

        public void OnCompleted() { }

        public void OnError(Exception e) { }

        public void OnNext(ContactCommand command)
        {
            if (command.IsList()) contacts = new List<Contact>(command.Contacts);
            if (command.IsAdd()) contacts.Add(command.Contact);
        }

        public IDisposable Subscribe(IObserver<ContactCommand> observer) => observers.Subscribe(observer);
    }

    public class ContactCommand
    {
        private ContactCommand() { }

        public ContactCommandType CommandType { get; private set; }

        public Contact Contact { get; private set; }

        public IEnumerable<Contact> Contacts { get; private set; }

        public bool IsGet() => CommandType == ContactCommandType.Get;

        public bool IsList() => CommandType == ContactCommandType.List;

        public bool IsAdd() => CommandType == ContactCommandType.Add;

        public bool IsRemove() => CommandType == ContactCommandType.Delete;

        public bool IsUpdate() => CommandType == ContactCommandType.Update;

        public static ContactCommand Get()
        {
            return new ContactCommand { CommandType = ContactCommandType.Get };
        }

        public static ContactCommand List(IEnumerable<Contact> contacts)
        {
            return new ContactCommand { CommandType = ContactCommandType.List, Contacts = contacts };
        }

        public static ContactCommand Add(Contact contact)
        {
            return new ContactCommand { CommandType = ContactCommandType.Add, Contact = contact };
        }

        public static ContactCommand Remove(Contact contact)
        {
            return new ContactCommand { CommandType = ContactCommandType.Delete, Contact = contact };
        }

        public static ContactCommand Update(IEnumerable<Contact> contacts)
        {
            return new ContactCommand { CommandType = ContactCommandType.Update, Contacts = contacts };
        }
    }

    public enum ContactCommandType { Get, List, Add, Delete, Update }
}
