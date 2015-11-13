using System;
using System.IO;

namespace Redcat.Core.Net
{
    public class StreamProxy : Stream
    {
        private Stream stream;

        public StreamProxy(Stream originStream)
        {
            if (originStream == null) throw new ArgumentNullException(nameof(originStream));
            stream = originStream;
        }

        public Stream OriginStream
        {
            get { return stream; }
            set
            {
                if (value == null) throw new ArgumentNullException();
                stream = value;
            }
        }

        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => stream.CanSeek;

        public override bool CanWrite => stream.CanWrite;

        public override long Length => stream.Length;

        public override long Position
        {
            get { return stream.Position; }
            set { stream.Position = value; }
        }

        public override void Flush() => stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => stream.Seek(offset, origin);

        public override void SetLength(long value) => stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => stream.Write(buffer, offset, count);
    }
}
