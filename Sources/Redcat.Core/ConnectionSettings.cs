namespace Redcat.Core
{
    public class ConnectionSettings : PropertySet
    {
        public string Name { get; set; }
                
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

        public int Port
        {
            get { return GetInt32("Port"); }
            set { Set("Port", value); }
        }
    }
}
