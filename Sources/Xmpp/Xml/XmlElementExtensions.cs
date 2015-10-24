using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Redcat.Xmpp.Xml
{
    public static class XmlElementExtensions
    {
        public static bool HasChild(this XmlElement element, string name, string xmlns = null)
        {
            return HasElement(element.Childs, name, xmlns);
        }

        public static XmlElement Child(this XmlElement element, string name, string xmlns = null)
        {
            return Element(element.Childs, name, xmlns);
        }

        public static bool HasElement(this IEnumerable<XmlElement> elements, string name, string xmlns = null)
        {
            if (IsNullOrEmpty(xmlns)) return elements.Any(e => e.Name == name);
            return elements.Any(e => e.Name == name && e.Xmlns == xmlns);
        }

        public static XmlElement Element(this IEnumerable<XmlElement> elements, string name, string xmlns = null)
        {
            if (IsNullOrEmpty(xmlns)) return elements.FirstOrDefault(e => e.Name == name);
            return elements.FirstOrDefault(e => e.Name == name && e.Xmlns == xmlns);
        }
    }
}
