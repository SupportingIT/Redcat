using System;

namespace Redcat.Core.Channels
{
    public interface IReactiveInputChannel<T> : IObservable<T>
    {
        void Receive();
    }
}
