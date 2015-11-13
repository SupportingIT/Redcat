using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using System.Configuration;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppIntegrationTests
    {
        [Test]
        public void Client_Connection()
        {
            string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            Communicator communicator = new Communicator();
            communicator.AddXmppExtension();
            communicator.AddNetworkExtension();
            communicator.Run();

            ConnectionSettings settings = CreateConnectionSettings();
            communicator.Connect(settings);
        }        

        private ConnectionSettings CreateConnectionSettings()
        {
            ConnectionSettings settings = new ConnectionSettings();
            settings.ChannelType = "xmpp";
            settings.Domain = ConfigurationManager.AppSettings["Domain"];
            settings.Host = ConfigurationManager.AppSettings["Host"];
            settings.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            settings.Username = ConfigurationManager.AppSettings["Username"];
            settings.Password = ConfigurationManager.AppSettings["Password"];
            return settings;
        }        
    }
}
