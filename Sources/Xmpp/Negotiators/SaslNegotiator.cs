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

        public SaslNegotiator(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            authenticators = new Dictionary<string, SaslAuthenticator>();
            this.settings = settings;
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
            XmlElement result = authenticator.Invoke(stream, settings);
            
            return true;
        }

        private SaslAuthenticator FindAuthenticator(ICollection<XmlElement> childs)
        {
            foreach (var mechanism in childs)
            {
                if (mechanism.Name != "mechanism") continue;                
                string authName =  mechanism.Value?.ToString();
                if (string.IsNullOrEmpty(authName)) continue;
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
