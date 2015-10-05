using System;

namespace Redcat.Xmpp.Xml
{
    public class PresenceStanza : Stanza
    {
        public static readonly int MinPriorityValue = -127;
        public static readonly int MaxPriorityValue = 128;

        private int? priority;

        public PresenceStanza() : base("presence")
        {}

        public PresenceStanza(string type) : base("presence")
        {
            Type = type;
        }        

        public int? Priority
        {
            get { return priority; }
            set
            {
                if(value != null && (value.Value > MaxPriorityValue || value.Value < MinPriorityValue))
                {
                    throw new ArgumentException();
                }
                priority = value;
            }
        }

        public string Show { get; set; }

        public string Status { get; set; }
    }
}
