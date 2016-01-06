using Redcat.Core.Channels;
using System;

namespace Redcat.Core
{
    public abstract class Connection : IDisposable
    {
        protected Connection(string name)
        {
            Name = name;            
        }

        public string Name { get; }

        public abstract bool IsOpen { get; }

        public void Close() => OnClosing();

        public void Dispose() => Close();

        protected virtual void OnClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

        internal event EventHandler Closing;
    }

    internal class ChannelConnection : Connection
    {
        private IChannel channel;

        internal ChannelConnection(string name, IChannel channel) : base(name)
        {
            this.channel = channel;
        }

        public override bool IsOpen => channel.State == ChannelState.Open;

        protected override void OnClosing()
        {
            base.OnClosing();
            channel.Close();
        }
    }
}
