using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Core.Net;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;
using SimpleInjector;
using System;
using System.Configuration;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppIntegrationTests
    {
        [Ignore]
        [Test]
        public void Test_Xmpp_Connection()
        {            
            ICommunicator communicator = CreateCommunicator();            
            ConnectionSettings settings = CreateConnectionSettings();

            communicator.Connect(settings);
            communicator.Send(Presence.Available());
        }        

        private ICommunicator CreateCommunicator()
        {
            Container container = new Container();

            container.Register<ICommunicator, SingleChannelCommunicator>();            
            container.Register<IChannelFactory, XmppChannelFactory>();
            container.Register<IMessageDispatcher, MessageDispatcher>();
            container.Register<IChannelFactory<IStreamChannel>, TcpChannelFactory>();
            container.Register<Func<ISaslCredentials>>(() => GetCredentials);

            return container.GetInstance<ICommunicator>();
        }

        private ConnectionSettings CreateConnectionSettings()
        {
            ConnectionSettings settings = new ConnectionSettings();            
            settings.Domain =  ConfigurationManager.AppSettings["Domain"];
            settings.Host = ConfigurationManager.AppSettings["Host"];
            settings.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            return settings;
        }

        private ISaslCredentials GetCredentials()
        {
            return new SaslCredentials(ConfigurationManager.AppSettings["Username"], 
                                       ConfigurationManager.AppSettings["Password"]);
        }

        private ISaslCredentials GetEmptyCredentials()
        {
            return new SaslCredentials("", "");
        }
    }    

    class SaslCredentials : ISaslCredentials
    {
        internal SaslCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Password { get; }

        public string Username { get; }

        public void Dispose()
        { }
    }
}
