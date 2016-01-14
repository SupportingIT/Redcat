using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class IqTest
    {
        private string xmlns = "some-ns";

        #region Creation methods tests

        [Test]
        public void Get_CreatesIqStanzaWithTypeGet()
        {
            IqStanza stanza = Iq.Get();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Get));
        }

        [Test]
        public void Set_CreatesIqStanzaWithSetType()
        {
            IqStanza stanza = Iq.Set();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Set));
        }

        [Test]
        public void Result_CreatesIqStanzaWithResultType()
        {
            IqStanza stanza = Iq.Result();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Result));
        }

        [Test]
        public void Error_CreatesIqStanzaWithErrorType()
        {
            IqStanza stanza = Iq.Error();

            Assert.That(stanza.Type, Is.EqualTo(Iq.Type.Error));
        }

        #endregion

        #region Is... methods tests

        [Test]
        public void IsGet_GetStanzaType_ReturnsTrue() 
        {
            VerifyIsMethod(Iq.IsGet, Iq.Type.Get, true);
        }

        [Test]
        public void IsGet_NotGetStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsGet, "not-get", false);
        }

        [Test]
        public void IsSet_SetStanzaType_ReturnsTrue()
        {
            VerifyIsMethod(Iq.IsSet, Iq.Type.Set, true);
        }

        [Test]
        public void IsSet_NotSetStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsSet, "not-set", false);
        }

        [Test]
        public void IsResult_ResultStanzaType_ReturnsTrue()
        {
            VerifyIsMethod(Iq.IsResult, Iq.Type.Result, true);
        }

        [Test]
        public void IsResult_NotResultStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsResult, "not-result", false);
        }

        [Test]
        public void IsError_ErrorStanzaType_ReturnsTrue()
        {
            VerifyIsMethod(Iq.IsError, Iq.Type.Error, true);
        }

        [Test]
        public void IsError_NotErrorStanzaType_ReturnsFalse()
        {
            VerifyIsMethod(Iq.IsError, "not-error", false);
        }

        private void VerifyIsMethod(Func<IqStanza, bool> isFunc, string type, bool expected)
        {
            IqStanza stanza = new IqStanza(type);
            Assert.That(isFunc(stanza), Is.EqualTo(expected));
        }

        #endregion
    }
}