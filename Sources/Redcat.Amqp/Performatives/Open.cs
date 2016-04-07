using System;

namespace Redcat.Amqp.Performatives
{
    public class Open
    {
        public string ContainerId { get; set; }

        public string Hostname { get; set; }

        public uint? MaxFrameSize { get; set; }

        public ushort? MaxChannel { get; set; }

        public TimeSpan? IdleTimeout { get; set; }
    }
}
