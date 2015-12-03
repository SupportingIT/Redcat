using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Communication;
using Redcat.Core.Net;
using Redcat.Xmpp.Communication;
using SimpleInjector;
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
            int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            ICommunicator communicator = CreateCommunicator();
            
            ConnectionSettings settings = CreateConnectionSettings();
            communicator.Connect(settings);

            //communicator.Send<Stanza>(Presence.Available());
        }

        private ICommunicator CreateCommunicator()
        {
            Container container = new Container();

            container.Register<ICommunicator, Communicator>();
            container.Register<IChannelManager, ChannelManager>();
            container.RegisterCollection<IChannelFactory>(new[] { typeof(XmppChannelFactory) });
            container.Register<IChannelFactory<IStreamChannel>, TcpChannelFactory>();
            container.Register<IMessageDispatcher, MessageDispatcher>();

            return container.GetInstance<ICommunicator>();
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
            settings.Resource(ConfigurationManager.AppSettings["Resource"]);
            return settings;
        }        
    }
}
