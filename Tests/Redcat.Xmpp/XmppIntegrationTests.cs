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
using System.Linq;
using System.Threading;

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
            ExecuteConnected(c => { });
        }

        [Ignore]
        [Test]
        public void TestRosterRequest()
        {            
            ExecuteConnected(c => {
                var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                var comm = c as SingleChannelCommunicator;
                var channel = comm.Channel as XmppChannel;
                Contact contact = new RosterItem("test_contact2@redcat.com");

                ContactController controller = ConnectContactController(channel);
                controller.Remove(contact);
                var roster = channel.Read();

                CollectionAssert.IsNotEmpty(controller.Contacts);
            });
        }

        private void ExecuteConnected(Action<ICommunicator> action)
        {
            ICommunicator communicator = CreateCommunicator();
            ConnectionSettings settings = CreateConnectionSettings();

            communicator.Connect(settings);
            action(communicator);
            communicator.Disconnect();
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

        private ICommunicator CreateCommunicator()
        {
            RegisterCommunicator();
            RegisterMessageHandlers();
            return container.GetInstance<ICommunicator>();
        }

        private void RegisterCommunicator()
        {
            container.RegisterSingleton<ICommunicator, SingleChannelCommunicator>();
            container.RegisterSingleton<IChannelFactory, XmppChannelFactory>();
            container.RegisterSingleton<IMessageDispatcher, MessageDispatcher>();
            container.RegisterSingleton<IChannelFactory<IStreamChannel>, TcpChannelFactory>();
            container.RegisterSingleton<Func<ISaslCredentials>>(() => GetCredentials);
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
