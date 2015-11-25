using Redcat.Core;

namespace Redcat.Xmpp
{
    public static class ConnectionSettingExtension
    {
        public static string Resource(this ConnectionSettings settings)
        {
            return settings.GetString("Xmpp.Resource");
        }

        public static void Resource(this ConnectionSettings settings, string value)
        {
            settings.Set("Xmpp.Resource", value);
        }

        public static JID UserJid(this ConnectionSettings settings)
        {
            return settings.Get<JID>("Xmpp.UserJid");
        }

        public static void UserJid(this ConnectionSettings settings, JID value)
        {
            settings.Set("Xmpp.UserJid", value);
        }
    }
}
