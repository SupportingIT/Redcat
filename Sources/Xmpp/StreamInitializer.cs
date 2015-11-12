﻿using System.Collections.Generic;
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

        public void Start(IXmppStream stream)
        {
            bool sendHeader = true;

            for (int i = 0; i < iterationLimit; i++)
            {
                if (sendHeader) ExchangeHeaders(stream);

                var response = stream.Read();
                VerifyStreamFeatures(response);

                if (response.Childs.Count == 0) return;
                if (!HasNegotiatorForFeatures(response.Childs)) return;

                sendHeader = HandleFeatures(stream, response.Childs);
            }
            throw new InvalidOperationException();
        }

        private void ExchangeHeaders(IXmppStream stream)
        {
            StreamHeader header = StreamHeader.CreateClientHeader(settings.Domain);            
            stream.Write(header);
            var response = stream.Read();
            VerifyResponseHeader(response);
        }

        private void VerifyResponseHeader(XmlElement response)
        {
            //var fromJid = response.GetAttributeValue<string>("from");
            if (response.Name != "stream:stream") throw new ProtocolViolationException();
            if (response.Xmlns != Namespaces.JabberClient) throw new ProtocolViolationException("Invalid xmlns for client header");
            //ejabbert doesn't set xmlns:stream attribute
            //if (response.GetAttributeValue<string>("xmlns:stream") != Namespaces.Streams) throw new ProtocolViolationException();
        }

        private bool HasNegotiatorForFeatures(IEnumerable<XmlElement> features)
        {
            return features.Any(f => negotiators.Any(n => n.CanNeogatiate(f)));
        }

        private bool HandleFeatures(IXmppStream stream, ICollection<XmlElement> features)
        {
            var feature = SelectFeature(features);
            if (negotiators.Any(n => n.CanNeogatiate(feature)))
            {
                var negotiator = negotiators.First(n => n.CanNeogatiate(feature));
                return negotiator.Neogatiate(stream, feature);
            }
            return false;
        }
        
        private XmlElement SelectFeature(IEnumerable<XmlElement> features)
        {
            if (features.HasTlsFeature()) return features.TlsFeature();
            return features.First();
        }

        private void VerifyStreamFeatures(XmlElement response)
        {
        }
    }
}
