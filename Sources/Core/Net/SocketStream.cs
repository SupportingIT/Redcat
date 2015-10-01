using System;
using System.IO;

namespace Redcat.Core.Net
{
    public class SocketStream : Stream
    {
        private ISocket socket;

        public SocketStream(ISocket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            this.socket = socket;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position { get; set; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (socket.Available == 0) return 0;
            return socket.Receive(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            socket.Send(buffer, offset, count);
        }
    }
}
