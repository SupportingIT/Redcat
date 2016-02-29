using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Negotiators;
using SimpleInjector;
using System;
using System.Configuration;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppIntegrationTests
    {
        private Container container;        

        [SetUp]
        public void SetUp()
        {
            container = new Container();
        }       

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        [Ignore]
        [Test]
        public void TestXmppConnection()
        {
            
        }

        [Ignore]
        [Test]
        public void TestRosterRequest()
        {            
            
        }

        private void ExecuteConnected(Action action)
        {
            //ICommunicator communicator = CreateCommunicator();
            //ConnectionSettings settings = CreateConnectionSettings();

            //communicator.Connect(settings);
            //action(communicator);
            //communicator.Disconnect();
        }

        private ContactController ConnectContactController(XmppChannel channel)
        {
            ContactController controller = container.GetInstance<ContactController>();
            RosterHandler handler = container.GetInstance<RosterHandler>();
            StanzaRouter router = container.GetInstance<StanzaRouter>();

            channel.Subscribe(router);
            router.Subscribe(handler);
            handler.Subscribe(channel);
            handler.Subscribe(controller);
            controller.Subscribe(handler);

            return controller;
        }        

        private void RegisterMessageHandlers()
        {
            container.RegisterSingleton<StanzaRouter>();
            container.RegisterSingleton<RosterHandler>();
            container.RegisterSingleton(() => new ContactController());
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
