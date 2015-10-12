using System.Collections.Generic;
using System.Linq;
using Redcat.Core;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public class StreamInitializer : IStreamInitializer
    {
        private ICollection<IFeatureNegatiator> negotiators;
        private ConnectionSettings settings;

        public StreamInitializer(ConnectionSettings settings)
        {
            this.settings = settings;
            negotiators = new List<IFeatureNegatiator>();
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
            SendHeader(stream);
            var response = stream.Read();
            VerifyResponseHeader(response);

            response = stream.Read();
            VerifyStreamFeatures(response);
            foreach (var feature in response.Childs)
            {
                var negotiator = negotiators.First(n => n.CanNeogatiate(feature));
                negotiator.Neogatiate(stream, feature);
            }
        }

        private void SendHeader(IXmppStream stream)
        {
            StreamHeader header = StreamHeader.CreateClientHeader(settings.Domain);
            stream.Write(header);
        }

        private void VerifyResponseHeader(XmlElement response)
        {
        }

        private void VerifyStreamFeatures(XmlElement response)
        {
        }
    }
}
