using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Channels
{
    public class ReactiveXmppChannel : IReactiveInputChannel<Stanza>
    {
        private IReactiveInputChannel<BinaryData> dataChannel;        

        public void Receive() { }

        public IDisposable Subscribe(IObserver<Stanza> observer)
        {
            throw new NotImplementedException();
        }
    }
}
