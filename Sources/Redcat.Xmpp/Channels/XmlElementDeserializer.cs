using Redcat.Xmpp.Xml;
using Redcat.Xmpp.Parsing;
using Redcat.Core.Serializaton;

namespace Redcat.Xmpp.Channels
{
    public class XmlElementDeserializer : ReactiveDeserializerBase<XmlElement>
    {
        private IXmlParser parser;

        public XmlElementDeserializer(IXmlParser parser, int bufferSize) : base(bufferSize)
        {
            this.parser = parser;
        }

        protected override void OnBufferUpdated()
        {
            for (int i = Buffer.Count - 1; i >= 0; i--)
            {
                if (Buffer[i] == '>')
                {
                    var elements = parser.Parse(Buffer.ToString(0, i + 1));                    
                    Buffer.Discard(i + 1);
                    break;
                }
            }
        }
    }
}
