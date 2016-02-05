using System;
using Redcat.Xmpp.Xml;
using System.Collections.Generic;

namespace Redcat.Xmpp.Negotiators
{
    public class SaslNegotiator : IFeatureNegatiator
    {
        private IDictionary<string, SaslAuthenticator> authenticators;
        private Func<ISaslCredentials> credentialProvider;

        public SaslNegotiator(Func<ISaslCredentials> credentialProvider)
        {
            if (credentialProvider == null) throw new ArgumentNullException(nameof(credentialProvider));
            authenticators = new Dictionary<string, SaslAuthenticator>();
            this.credentialProvider = credentialProvider;
        }

        public void AddAuthenticator(string mechanismName, SaslAuthenticator authenticator)
        {
            authenticators.Add(mechanismName, authenticator);
        }

        public bool CanNegotiate(NegotiationContext context, XmlElement feature)
        {
            using (var credentials = credentialProvider.Invoke())
            {
                return !context.IsAuthenticated && IsSaslFeature(feature) && IsCredentialsValid(credentials);
            }
        }

        private bool IsCredentialsValid(ISaslCredentials credentials)
        {
            return credentials != null && !string.IsNullOrEmpty(credentials.Username) && !string.IsNullOrEmpty(credentials.Password);
        }

        public bool Negotiate(NegotiationContext context, XmlElement feature)
        {            
            if (!IsSaslFeature(feature)) throw new InvalidOperationException();
            if (feature.Childs.Count == 0) throw new InvalidOperationException();

            SaslAuthenticator authenticator = FindAuthenticator(feature.Childs);
            using (var credentials = credentialProvider.Invoke())
            {
                XmlElement result = authenticator.Invoke(context.Stream, credentials);
                if (result.Name == "failure") throw new InvalidOperationException();
                if (result.Name == "success") context.IsAuthenticated = true;
            }
            
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
