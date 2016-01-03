using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public class Connection : IDisposable
    {
        private IChannel channel;

        internal Connection(string name, IChannel channel)
        {
            Name = name;
            this.channel = channel;
        }

        public string Name { get; }

        public bool IsOpen { get; }

        public void Close() { }

        public void Dispose() { }
    }
}
