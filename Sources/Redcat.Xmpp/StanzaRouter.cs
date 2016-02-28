using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class StanzaRouter : IObserver<Stanza>, IObservable<IqStanza>, IObservable<PresenceStanza>
    {
        private ICollection<IObserver<IqStanza>> iqObservers;
        private ICollection<IObserver<PresenceStanza>> presenceObservers;

        public StanzaRouter()
        {
            iqObservers = new List<IObserver<IqStanza>>();
            presenceObservers = new List<IObserver<PresenceStanza>>();
        }

        public void OnCompleted()
        {
            iqObservers.OnCompleted();
            presenceObservers.OnCompleted();
        }

        public void OnError(Exception error)
        { }

        public void OnNext(Stanza stanza)
        {
            if (stanza is IqStanza) iqObservers.OnNext((IqStanza)stanza);
            if (stanza is PresenceStanza) presenceObservers.OnNext((PresenceStanza)stanza);
        }

        public IDisposable Subscribe(IObserver<PresenceStanza> observer)
        {
            return presenceObservers.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<IqStanza> observer)
        {
            return iqObservers.Subscribe(observer);
        }
    }
}
