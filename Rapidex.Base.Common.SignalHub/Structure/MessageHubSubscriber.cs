using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex.MessageHub;
internal class MessageHubSubscriber
{
    public string ClientId { get; }
    public int Id { get; set; }
    public MessageTopic Topic { get; set; }
    internal List<string> TopicSectionsForLocate { get; set; }
    internal int TopicSectionLevelForLocate { get; set; } = 0;
    public Func<IMessageArguments, IMessageArguments> Handler { get; set; }

    public MessageHubSubscriber(string clientId, int handlerId, MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler)
    {
        topic.Check();

        this.ClientId = clientId;
        Id = handlerId;
        Topic = topic;
        TopicSectionsForLocate = new List<string>(topic.Sections);
        Handler = handler;
    }
}
