using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;

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
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IqStanza value)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ContactCommand command)
        {
            if (command.IsGet()) iqObservers.OnNext(Roster.Request());
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
