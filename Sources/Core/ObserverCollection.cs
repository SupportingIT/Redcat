using System;
using System.Collections.Generic;

namespace Redcat.Core
{
    public class ObserverCollection<T> : List<IObserver<T>>
    {
        public void OnCompleted()
        {
            foreach (var observer in this) observer.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var observer in this) observer.OnError(error);
        }

        public void OnNext(T value)
        {
            foreach(var observer in this) observer.OnNext(value);            
        }

        
    }
}
