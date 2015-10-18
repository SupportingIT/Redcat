namespace Redcat.Core
{
    public class ConnectionSettings : PropertySet
    {
        public string Domain
        {
            get { return GetString("Domain"); }
            set { Set("Domain", value); }
        }

        public string Host
        {
            get { return GetString("Host"); }
            set { Set("Host", value); }
        }

        public string Username
        {
            get { return GetString("Username"); }
            set { Set("Username", value); }
        }

        public string Password
        {
            get { return GetString("Password"); }
            set { Set("Password", value); }
        }

        public int Port
        {
            get { return GetInt32("Port"); }
            set { Set("Port", value); }
        }        
    }
}
