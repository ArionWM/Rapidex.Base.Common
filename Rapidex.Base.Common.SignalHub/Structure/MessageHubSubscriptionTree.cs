using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex.MessageHub;

internal class MessageHubSubscriptionTreeItem
{
    public string Section { get; set; }
    public List<MessageHubSubscriber> Subscribers { get; set; } = new List<MessageHubSubscriber>();
    public DictionaryA<MessageHubSubscriptionTreeItem> Items { get; } = new DictionaryA<MessageHubSubscriptionTreeItem>();

    public MessageHubSubscriptionTreeItem(string section)
    {
        Section = section;
    }
    public virtual void Add(MessageHubSubscriber subscriber)
    {
        //`<tenantShortName>/<workspace>/<module>/<message>/<entityName>/<entityId>/<fieldName>`
        //Level0 / 1 / 2 / 3 / 4 / 5 / 6
        //# wildcard support after level 3

        if (subscriber.TopicSectionsForLocate.Any())
        {
            string section = subscriber.TopicSectionsForLocate[0].Trim();
            subscriber.TopicSectionsForLocate.RemoveAt(0);

            if (subscriber.TopicSectionLevelForLocate < 4 && section == "#")
            {
                //Wildcard, but not allowed before level 4
                throw new InvalidOperationException("Wildcard '#' is not allowed before level 4 in topic sections.");
            }

            subscriber.TopicSectionLevelForLocate++;

            MessageHubSubscriptionTreeItem item = this.Items.GetOr(section, () =>
            {
                //Yeni bir bölüm
                var item = new MessageHubSubscriptionTreeItem(section);
                return item;
            });

            item.Add(subscriber);
        }
        else
        {
            this.Subscribers.Add(subscriber);
        }
    }

    public MessageHubSubscriber[] GetSubscribers(string[] sections)
    {
        //`<tenantShortName>/<workspace>/<module>/<message>/<entityName>/<entityId>/<fieldName>`
        //Level0 / 1 / 2 / 3 / 4 / 5 / 6

        //if (sections.IsNullOrEmpty())
        //    return this.Subscribers.ToArray();

        List<string> _sections = new List<string>(sections);

        List<MessageHubSubscriber> subscribers = new List<MessageHubSubscriber>();

        string section = _sections[0];
        _sections.RemoveAt(0);


        //TODO: yayınlanan topiclerde sections içerisinde wildcard olabilir mi?
        if (SignalConstants.WildcardStrs.Contains(section))
        {
            var subSections = _sections.ToArray();
            //deeper for all
            foreach (var item in this.Items)
            {
                subscribers.AddRange(item.Value.GetSubscribers(subSections));
            }
        }
        else
        {
            //Bire bir uyuşanlar
            MessageHubSubscriptionTreeItem subItemDirect = this.Items.Get(section);
            if (subItemDirect != null)
            {
                //Kısa kalanlar (# yerine geçer)
                subscribers.AddRange(subItemDirect.Subscribers);

                //Alt section'lara abone olanlar
                if (_sections.Any())
                    subscribers.AddRange(subItemDirect.GetSubscribers(_sections.ToArray()));
            }

            //Wildcard'ı olanlar
            MessageHubSubscriptionTreeItem subItemSectionWildcard = this.Items.Get("+");
            if (subItemSectionWildcard != null)
            {
                if (!_sections.Any())
                    subscribers.AddRange(subItemSectionWildcard.Subscribers);
                else
                    subscribers.AddRange(subItemSectionWildcard.GetSubscribers(_sections.ToArray()));
            }

            MessageHubSubscriptionTreeItem subItemAllWildcard = this.Items.Get("#");
            if (subItemAllWildcard != null)
            {
                subscribers.AddRange(subItemAllWildcard.Subscribers);
            }
        }

        return subscribers.ToArray();
    }


    public MessageHubSubscriber[] GetSubscribers(MessageTopic topic)
    {
        return this.GetSubscribers(topic.Sections);
    }
}

internal class MessageHubSubscriptionTree : MessageHubSubscriptionTreeItem
{
    protected Dictionary<int, MessageHubSubscriber> subscriberHandlerIndex = new Dictionary<int, MessageHubSubscriber>();

    public MessageHubSubscriptionTree() : base(null)
    {
    }

    public override void Add(MessageHubSubscriber subscriber)
    {
        subscriber.TopicSectionsForLocate.NotEmpty("Subscriber's topic sections cannot be empty.");
        this.subscriberHandlerIndex.Set(subscriber.Id, subscriber);
        base.Add(subscriber);
    }

    public virtual void Add(string clientId, int handlerId, MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler)
    {
        var subscriber = new MessageHubSubscriber(clientId, handlerId, topic, handler);
        this.Add(subscriber);
    }

    public void Remove(MessageHubSubscriber subscriber)
    {
        throw new NotImplementedException();
        ////this.subscriberHandlerIndex.Remove(subscriber.Id);
        //base.Remove(subscriber);
    }

    public void Remove(int subscriberId)
    {
        throw new NotImplementedException();
        ////this.subscriberHandlerIndex.Remove(subscriber.Id);
        //base.Remove(subscriber);
    }
}
