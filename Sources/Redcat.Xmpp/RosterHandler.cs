using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp
{
    public class RosterHandler : IObserver<IqStanza>, IObserver<ContactCommand>, IObservable<ContactCommand>
    {
        private ICollection<IObserver<ContactCommand>> contactObservers;
        private ICollection<IObserver<IqStanza>> iqObservers;

        public RosterHandler()
        {
            contactObservers = new List<IObserver<ContactCommand>>();
            iqObservers = new List<IObserver<IqStanza>>();
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(IqStanza value)
        {
            if (value.IsResult())
            {
                var items = value.GetRosterItems();
                var contacts = items.Select(i => {
                    var jid = i.Attributes.FirstOrDefault(a => a.Name == "jid").ToString();
                    RosterItem item = new RosterItem(jid);
                    item.Name = i.GetAttributeValue<string>("name");
                    return item;
                });
                contactObservers.OnNext(ContactCommand.List(contacts));
            }
        }

        public void OnNext(ContactCommand command)
        {
            if (command.IsGet()) iqObservers.OnNext(Roster.Request());
            if (command.IsAdd()) iqObservers.OnNext(Roster.AddItem(command.Contact));
            if (command.IsRemove()) iqObservers.OnNext(Roster.RemoveItem(command.Contact));
        }

        public IDisposable Subscribe(IObserver<ContactCommand> observer)
        {
            return contactObservers.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<IqStanza> observer)
        {
            return iqObservers.Subscribe(observer);
        }
    }
}
