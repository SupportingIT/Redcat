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
    [Category("Integration")]
    [TestFixture]
    public class XmppCommunicatorIntegrationTests
    {
        private Container container;        

        [SetUp]
        public void SetUp()
        {
            container = new Container();
            RegisterCommunicator();
        }       

        private void RegisterCommunicator()
        {
            container.RegisterSingleton<XmppCommunicator>();
            container.RegisterSingleton<IXmppChannelFactory, XmppChannelFactory>();
            container.RegisterSingleton<IStreamChannelFactory, TcpChannelFactory>();
            container.RegisterSingleton<Func<ISaslCredentials>>(() => GetCredentials);
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
            var communicator = container.GetInstance<XmppCommunicator>();
            var settings = CreateConnectionSettings();

            communicator.Connect(settings);
            Assert.That(communicator.IsConnected, Is.True);
            communicator.Disconnect();
        }

        [Ignore]
        [Test]
        public void TestRosterRequest()
        {
            var communicator = container.GetInstance<XmppCommunicator>();
            var settings = CreateConnectionSettings();
            communicator.Connect(settings);

            //communicator.LoadContacts();
            //communicator.AddContact("user1@redcat", "User1");
            //communicator.RemoveContact("user1@redcat");
            communicator.WaitIncominMessage();

            //CollectionAssert.IsNotEmpty(communicator.Contacts);
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
            //return new SaslCredentials("user1@redcat", "123");
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
