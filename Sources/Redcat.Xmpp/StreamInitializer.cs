using System.Collections.Generic;
using System.Linq;
using Redcat.Core;
using Redcat.Xmpp.Xml;
using System.Net;
using System;
using Redcat.Core.Channels;

namespace Redcat.Xmpp
{
    public class StreamInitializer
    {
        private ICollection<IFeatureNegatiator> negotiators;
        private ConnectionSettings settings;
        private NegotiationContext context;
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

        public void Init(IDuplexChannel<XmlElement> stream)
        {
            bool initRequired = true;
            context = null;// new NegotiationContext(stream);
            XmlElement features = null;
            
            for (int i = 0; i < iterationLimit; i++)
            {
                if (initRequired)
                {
                    InitStream(stream);
                    features = ReadFeatures(stream);
                }

                if (features.Childs.Count == 0) return;
                if (!HasNegotiatorForFeatures(features.Childs)) return;                

                initRequired = HandleFeatures(stream, features.Childs);                
            }
            throw new InvalidOperationException();
        }

        private void InitStream(IDuplexChannel<XmlElement> stream)
        {
            StreamHeader header = StreamHeader.CreateClientHeader(settings.Domain);
            stream.Send(header);
            XmlElement response = stream.Receive();
            //First response might be a result of previous feature negotiation
            if (response.Name != "stream:stream") response = stream.Receive();
            VerifyResponseHeader(response);
        }

        private XmlElement ReadFeatures(IDuplexChannel<XmlElement> stream)
        {
            XmlElement features = stream.Receive();
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
            return features.Any(f => negotiators.Any(n => n.CanNegotiate(context, f)));
        }

        private bool HandleFeatures(IDuplexChannel<XmlElement> stream, ICollection<XmlElement> features)
        {            
            XmlElement feature = null;
            IFeatureNegatiator negotiator = GetNegotiator(features, out feature);

            return negotiator.Negotiate(context, feature);
        }

        private IFeatureNegatiator GetNegotiator(ICollection<XmlElement> features, out XmlElement feature)
        {
            if (features.HasTlsFeature() && negotiators.Any(n => n.CanNegotiate(context, features.TlsFeature())))
            {
                feature = features.TlsFeature();
                return negotiators.First(n => n.CanNegotiate(context, features.TlsFeature()));
            }

            if (features.HasSaslFeature() && negotiators.Any(n => n.CanNegotiate(context, features.SaslFeature())))
            {
                feature = features.SaslFeature();
                return negotiators.First(n => n.CanNegotiate(context, features.SaslFeature()));
            }

            foreach (XmlElement f in features)
            {                
                var negotiator = negotiators.FirstOrDefault(n => n.CanNegotiate(context, f));
                if (negotiator != null)
                {
                    feature = f;
                    return negotiator;
                }
            }

            feature = null;
            return null;
        }

        private void VerifyStreamFeatures(XmlElement features)
        {
            if (features.Name != "stream:features") throw new ProtocolViolationException();
        }
    }
}
