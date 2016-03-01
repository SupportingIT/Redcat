using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class RosterHandlerTests
    {
        private RosterHandler handler;
        private IObserver<IqStanza> observer;
        private IqStanza iq;                
    }
}
