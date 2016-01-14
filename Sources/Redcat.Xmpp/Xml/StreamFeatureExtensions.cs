using System.Collections.Generic;

namespace Redcat.Xmpp.Xml
{
    public static class StreamFeatureExtensions
    {
        public static bool HasTlsFeature(this IEnumerable<XmlElement> features)
        {
            return features.HasElement("starttls", Namespaces.Tls);
        }

        public static bool HasTlsFeature(this XmlElement element)
        {
            return HasTlsFeature(element.Childs);
        }

        public static XmlElement TlsFeature(this IEnumerable<XmlElement> features)
        {
            return features.Element("starttls", Namespaces.Tls);
        }

        public static XmlElement TlsFeature(this XmlElement element)
        {
            return TlsFeature(element.Childs);
        }

        public static bool HasSaslFeature(this IEnumerable<XmlElement> features)
        {
            return features.HasElement("mechanisms");
        }

        public static bool HasSaslFeature(this XmlElement element)
        {
            return HasSaslFeature(element.Childs);
        }

        public static XmlElement SaslFeature(this IEnumerable<XmlElement> features)
        {
            return features.Element("mechanisms");
        }

        public static XmlElement SaslFeature(this XmlElement element)
        {
            return SaslFeature(element.Childs);
        }
    }
}
