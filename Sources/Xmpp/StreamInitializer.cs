using System.Collections.Generic;
using System.Linq;
using Redcat.Core;
using Redcat.Xmpp.Xml;
using System.Net;
using System;

namespace Redcat.Xmpp
{
    public class StreamInitializer : IStreamInitializer
    {
        private ICollection<IFeatureNegatiator> negotiators;
        private ConnectionSettings settings;
        private int iterationLimit;

        public StreamInitializer(ConnectionSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            this.settings = settings;
            negotiators = new List<IFeatureNegatiator>();            
            iterationLimit = 100;
        }

        public int IterationLimit
        {
            get { return iterationLimit; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException();
                iterationLimit = value;
            }
        }

        public ICollection<IFeatureNegatiator> Negotiators
        {
            get { return negotiators; }
        }

        public void AddNegotiators(IEnumerable<IFeatureNegatiator> negatiators)
        {
            foreach (var negatiator in negatiators) this.negotiators.Add(negatiator);
        }

        public void Init(IXmppStream stream)
        {
            bool initRequired = true;

            for (int i = 0; i < iterationLimit; i++)
            {
                if (initRequired) InitStream(stream);

                var features = ReadFeatures(stream);

                if (features.Childs.Count == 0) return;
                if (!HasNegotiatorForFeatures(features.Childs)) return;                

                initRequired = HandleFeatures(stream, features.Childs);                
            }
            throw new InvalidOperationException();
        }

        private void InitStream(IXmppStream stream)
        {
            StreamHeader header = StreamHeader.CreateClientHeader(settings.Domain);
            stream.Write(header);
            XmlElement response = stream.Read();
            //First response might be a result of previous feature negotiation
            if (response.Name != "stream:stream") response = stream.Read();
            VerifyResponseHeader(response);
        }

        private XmlElement ReadFeatures(IXmppStream stream)
        {
            XmlElement features = stream.Read();
            VerifyStreamFeatures(features);
            return features;
        }

        private void VerifyResponseHeader(XmlElement response)
        {
            if (response.Name != "stream:stream") throw new ProtocolViolationException();
            if (response.Xmlns != Namespaces.JabberClient) throw new ProtocolViolationException("Invalid xmlns for client header");
            //ejabbert doesn't set xmlns:stream attribute
            //if (response.GetAttributeValue<string>("xmlns:stream") != Namespaces.Streams) throw new ProtocolViolationException();
        }

        private bool HasNegotiatorForFeatures(IEnumerable<XmlElement> features)
        {
            return features.Any(f => negotiators.Any(n => n.CanNegotiate(f)));
        }

        private bool HandleFeatures(IXmppStream stream, ICollection<XmlElement> features)
        {
            var feature = SelectFeature(features);
            if (negotiators.Any(n => n.CanNegotiate(feature)))
            {
                var negotiator = negotiators.First(n => n.CanNegotiate(feature));                
                return negotiator.Negotiate(stream, feature);
            }
            return false;
        }
        
        private XmlElement SelectFeature(IEnumerable<XmlElement> features)
        {
            if (features.HasTlsFeature()) return features.TlsFeature();
            if (features.HasSaslFeature()) return features.SaslFeature();
            return features.First();
        }

        private void VerifyStreamFeatures(XmlElement features)
        {
            if (features.Name != "stream:features") throw new ProtocolViolationException();
        }
    }
}
