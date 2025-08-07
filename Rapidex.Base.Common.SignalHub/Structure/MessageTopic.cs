//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rapidex.MessageHub;

///// <summary>
///// <![CDATA[<tenantShortName>/<workspace>/<module>/<signal>/<signal type specific>]]>
///// </summary>
//public class MessageTopic : ICloneable, IComparable<MessageTopic>, IEquatable<MessageTopic>
//{
//    public string Tenant { get; set; }
//    public string Workspace { get; set; }
//    public string Module { get; set; }
//    public string Signal { get; set; }
//    public string Entity { get; set; }
//    public string EntityId { get; set; }
//    public string Field { get; set; }

//    public bool IsSystemLevel { get; set; }

//    public ISignalDefinition SignalDefinition { get; set; } = null;

//    public string[] Sections { get; set; } = new string[0];
//    public List<string> OtherSections { get; set; } = new List<string>();

//    public int HandlerId { get; set; } = 0;

//    public object Clone()
//    {

//        return new MessageTopic
//        {
//            Tenant = this.Tenant,
//            Workspace = this.Workspace,
//            Module = this.Module,
//            Signal = this.Signal,
//            Entity = this.Entity,
//            EntityId = this.EntityId,
//            Field = this.Field,
//            IsSystemLevel = this.IsSystemLevel,
//            SignalDefinition = this.SignalDefinition,
//            Sections = (string[])this.Sections.Clone(),
//            OtherSections = new List<string>(this.OtherSections)
//        };
//    }

//    public int CompareTo(MessageTopic? other)
//    {
//        return string.Compare(this.ToString(), other?.ToString(), StringComparison.OrdinalIgnoreCase);
//    }

//    public bool Equals(MessageTopic? other)
//    {
//        if (other == null)
//            return false;
//        return this.ToString().Equals(other.ToString(), StringComparison.OrdinalIgnoreCase);
//    }

//    public override string ToString()
//    {
//        string topic = $"{Tenant}/{Workspace}/{Module}/{Signal}";
//        if (Entity.IsNOTNullOrEmpty())
//        {
//            topic += $"/{Entity}";
//        }

//        if (EntityId.IsNOTNullOrEmpty())
//        {
//            topic += $"/{EntityId}";
//        }

//        if (Field.IsNOTNullOrEmpty())
//        {
//            topic += $"/{Field}";
//        }

//        if (this.OtherSections.Any())
//        {
//            topic += $"/{string.Join("/", this.OtherSections)}";
//        }

//        return topic;
//    }

//    public static implicit operator string(MessageTopic topic)
//    {
//        return topic.ToString();
//    }

//    public static implicit operator MessageTopic(string topicText)
//    {
//        TopicParser.TopicParseResult result = TopicParser.Parse(AppTest.Signals, topicText);
//        if (result.Valid)
//        {
//            return result.Topic;
//        }
//        else
//        {
//            throw new Exception($"Invalid topic: {topicText}");
//        }
//    }
//}
