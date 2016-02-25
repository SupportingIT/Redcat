using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redcat.Core.Channels
{
    public class BufferChannel<T> : ChannelBase, IInputChannel<T>, IAsyncInputChannel<T>, IObservable<T>, IObserver<ArraySegment<byte>>
    {
        private ByteBuffer buffer;
        private ICollection<T> observers;
        private TaskCompletionSource<T> completionSource;

        public BufferChannel(int bufferSize, ConnectionSettings settings) : base(settings)
        {
            buffer = new ByteBuffer(bufferSize);
            completionSource = new TaskCompletionSource<T>();
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(ArraySegment<byte> value)
        {
            buffer.Write(value.Array, value.Offset, value.Count);
            throw new NotImplementedException();
        }

        public T Receive()
        {
            return ReceiveAsync().Result;
        }

        public async Task<T> ReceiveAsync()
        {
            return await completionSource.Task;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }
}
