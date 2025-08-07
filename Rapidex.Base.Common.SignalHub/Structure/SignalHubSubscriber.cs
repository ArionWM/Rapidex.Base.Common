using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex.SignalHub;
internal class SignalHubSubscriber
{
    public string ClientId { get; }
    public int Id { get; set; }
    public SignalTopic Topic { get; set; }
    internal List<string> TopicSectionsForLocate { get; set; }
    internal int TopicSectionLevelForLocate { get; set; } = 0;
    public Func<ISignalArguments, ISignalArguments> Handler { get; set; }

    public SignalHubSubscriber(string clientId, int handlerId, SignalTopic topic, Func<ISignalArguments, ISignalArguments> handler)
    {
        topic.Check();

        this.ClientId = clientId;
        Id = handlerId;
        Topic = topic;
        TopicSectionsForLocate = new List<string>(topic.Sections);
        Handler = handler;
    }
}
