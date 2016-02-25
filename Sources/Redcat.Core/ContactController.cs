using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class ContactController : IObserver<ContactControllerCommand>
    {
        public IEnumerable<Contact> Contacts { get; }
        public void Add(Contact contact) { }
        public void Remove(Contact contact) { }
        public void Update() { }

        public void OnCompleted() { }
        public void OnError(Exception e) { }
        public void OnNext(ContactControllerCommand command) { }
    }

    public class ContactControllerCommand
    {
        public ContactControllerCommandType CommandType { get; set; }
        public Contact Contact { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }

    public enum ContactControllerCommandType { }
}
