using System;
using Redcat.Xmpp.Xml;
using Redcat.Core;
using System.Collections.Generic;

namespace Redcat.Xmpp.Negotiators
{
    public class SaslNegotiator : IFeatureNegatiator
    {
        private IDictionary<string, SaslAuthenticator> authenticators;
        private ConnectionSettings settings;

        public SaslNegotiator()
        {
            authenticators = new Dictionary<string, SaslAuthenticator>();
        }

        public void AddAuthenticator(string mechanismName, SaslAuthenticator authenticator)
        {
            authenticators.Add(mechanismName, authenticator);
        }

        public bool CanNeogatiate(XmlElement feature)
        {
            return IsSaslFeature(feature);
        }

        public bool Neogatiate(IXmppStream stream, XmlElement feature)
        {            
            if (!IsSaslFeature(feature)) throw new InvalidOperationException();
            if (feature.Childs.Count == 0) throw new InvalidOperationException();

            SaslAuthenticator authenticator = FindAuthenticator(feature.Childs);
            authenticator(stream, settings);
            
            return true;
        }

        private SaslAuthenticator FindAuthenticator(ICollection<XmlElement> childs)
        {
            foreach (var mechanism in childs)
            {                
                string authName =  mechanism.Value.ToString();
                if (authenticators.ContainsKey(authName)) return authenticators[authName];
            }
            throw new InvalidOperationException();
        }

        private bool IsSaslFeature(XmlElement feature)
        {
            return feature.Name == "mechanisms";
        }
    }

    
}
