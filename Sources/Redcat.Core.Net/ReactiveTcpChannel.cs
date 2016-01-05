using Redcat.Core.Channels;
using System;

namespace Redcat.Core.Net
{
    public class ReactiveTcpChannel : ChannelBase, IObservable<BinaryData>
    {
        public void Receive()
        { }

        public IDisposable Subscribe(IObserver<BinaryData> observer)
        {
            throw new NotImplementedException();
        }
    }
}
