using Redcat.Xmpp.Negotiators;
using System.Configuration;

namespace Redcat.App.Wpf.Services
{
    public class SaslCredentialsProvider
    {
        public ISaslCredentials GetCredentials()
        {
            return new SaslCredentials();
        }
    }

    class SaslCredentials : ISaslCredentials
    {        
        public string Username => ConfigurationManager.AppSettings["username"];
        public string Password => ConfigurationManager.AppSettings["password"];

        public void Dispose()
        { }
    }
}
