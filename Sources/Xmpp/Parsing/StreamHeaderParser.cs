using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public class StreamHeaderParser : DelegateParser<StreamHeader>
    {
        public StreamHeaderParser() : base(n => new StreamHeader(), "xml", "stream:stream")
        {
            this.Attribute("id", (e, v) => e.Id = v);
            this.Attribute("version", (e, v) => e.Version = v);
            this.Attribute("from", (e, v) => e.From = v);
            this.Attribute("xmlns", (e, v) => e.Xmlns = v);
            this.Attribute("xml:lang", (e, v) => e.XmlLang = v);
        }
    }
}
