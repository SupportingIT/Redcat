using System;

namespace Redcat.Xmpp.Xml
{
    public static class Presence
    {
        public static class ShowStatus
        {
            public static readonly string Away = "away";
            public static readonly string Chat = "chat";
            public static readonly string DoNotDisturb = "dnd";
            public static readonly string ExtendedAway = "xa";
        }

        public static class Type
        {
            public static readonly string Error = "error";
            public static readonly string Probe = "probe";
            public static readonly string Subscribe = "subscribe";
            public static readonly string Subscribed = "subscribed";
            public static readonly string Unavailable = "unavailable";
            public static readonly string Unsubscribe = "unsubscribe";
            public static readonly string Unsubscribed = "unsubscribed";
        }

        #region Creation methods

        public static PresenceStanza Available()
        {
            return new PresenceStanza();
        }

        public static PresenceStanza Unavailable()
        {
            return new PresenceStanza(Presence.Type.Unavailable);
        }

        public static PresenceStanza Subscribe()
        {
            return new PresenceStanza(Presence.Type.Subscribe);
        }

        public static PresenceStanza Subscribed()
        {
            return new PresenceStanza(Presence.Type.Subscribed);
        }

        public static PresenceStanza Unsubscribe()
        {
            return new PresenceStanza(Presence.Type.Unsubscribe);
        }

        public static PresenceStanza Unsubscribed()
        {
            return new PresenceStanza(Presence.Type.Unsubscribed);
        }

        public static PresenceStanza Error()
        {
            return new PresenceStanza(Presence.Type.Error);
        }

        public static PresenceStanza Probe()
        {
            return new PresenceStanza(Presence.Type.Probe);
        }

        #endregion

        #region Is.. methods

        public static bool IsAvailable(this PresenceStanza presence)
        {
            return string.IsNullOrEmpty(presence.Type);
        }

        public static bool IsUnavailable(this PresenceStanza presence)
        {
            return presence.Type == Type.Unavailable;
        }

        public static bool IsSubscribe(this PresenceStanza presence)
        {
            return presence.Type == Type.Subscribe;
        }

        public static bool IsSubscribed(this PresenceStanza presence)
        {
            return presence.Type == Type.Subscribed;
        }

        public static bool IsUnsubscribe(this PresenceStanza presence)
        {
            return presence.Type == Type.Unsubscribe;
        }

        public static bool IsUnsubscribed(this PresenceStanza presence)
        {
            return presence.Type == Type.Unsubscribed;
        }

        public static bool IsError(this PresenceStanza presence)
        {
            return presence.Type == Type.Error;
        }

        public static bool IsProbe(this PresenceStanza presence)
        {
            return presence.Type == Type.Probe;
        }

        #endregion

        #region Show methods

        public static PresenceStanza Show(this PresenceStanza presence, string showStatus)
        {
            presence.Show = showStatus;
            return presence;
        }

        public static PresenceStanza ShowAway(this PresenceStanza presence)
        {
            return presence.Show(ShowStatus.Away);
        }

        public static PresenceStanza ShowChat(this PresenceStanza presence)
        {
            return presence.Show(ShowStatus.Chat);
        }

        public static PresenceStanza ShowDoNotDisturb(this PresenceStanza presence)
        {
            return presence.Show(ShowStatus.DoNotDisturb);
        }

        public static PresenceStanza ShowExtendedAway(this PresenceStanza presence)
        {
            return presence.Show(ShowStatus.ExtendedAway);
        }

        #endregion

        public static PresenceStanza Status(this PresenceStanza presence, string status)
        {
            presence.Status = status;
            return presence;
        }
    }
}
