using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class StanzaRouter : IObserver<XmlElement>, IObservable<IqStanza>, IObservable<PresenceStanza>, IObservable<MessageStanza>
    {
        private ICollection<IObserver<IqStanza>> iqObservers;
        private ICollection<IObserver<PresenceStanza>> presenceObservers;
        private ICollection<IObserver<MessageStanza>> messageObservers;

        public StanzaRouter()
        {
            iqObservers = new List<IObserver<IqStanza>>();
            presenceObservers = new List<IObserver<PresenceStanza>>();
            messageObservers = new List<IObserver<MessageStanza>>();
        }

        public void OnCompleted()
        {
            iqObservers.OnCompleted();
            presenceObservers.OnCompleted();
            messageObservers.OnCompleted();
        }

        public void OnError(Exception error)
        { }

        public void OnNext(XmlElement element)
        {
            if (element is IqStanza) iqObservers.OnNext((IqStanza)element);
            if (element is PresenceStanza) presenceObservers.OnNext((PresenceStanza)element);
            if (element is MessageStanza) messageObservers.OnNext((MessageStanza)element);
        }

        public IDisposable Subscribe(IObserver<PresenceStanza> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            return presenceObservers.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<IqStanza> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            return iqObservers.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<MessageStanza> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            return messageObservers.Subscribe(observer);
        }
    }
}
