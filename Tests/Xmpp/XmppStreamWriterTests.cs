using Microsoft.XmlDiffPatch;
using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System.IO;
using System.Text;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppStreamWriterTests
    {
        [Test]
        public void Write_Writes_Stanza_To_Stream()
        {
            MemoryStream stream = new MemoryStream();
            XmppStreamWriter writer = new XmppStreamWriter(stream);
            Element element = null;

            writer.Write(element);

            string actualOutput = Encoding.UTF8.GetString(stream.ToArray());
            Assert.Fail("Unfinished test");
        }

        private XmlDiff CreateXmlDiff()
        {
            XmlDiff diff = new XmlDiff();
            diff.Algorithm = XmlDiffAlgorithm.Precise;
            diff.IgnoreWhitespace = true;
            diff.IgnoreChildOrder = true;
            return diff;
        }
    }
}
