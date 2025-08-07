using Rapidex.MessageHub;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex;

/// <summary>
/// <![CDATA[<tenantShortName>/<workspace>/<module>/<signal>/<signal type specific>]]>
/// </summary>
public class MessageTopic : ICloneable, IComparable<MessageTopic>, IEquatable<MessageTopic>
{
    public const string ANY = "+";
    public string Tenant { get; set; }
    public string Workspace { get; set; }
    public string Module { get; set; }
    public string Signal { get; set; }
    public string Entity { get; set; }
    public string EntityId { get; set; }
    public string Field { get; set; }

    public bool IsSystemLevel { get; set; }

    public bool IsSynchronous
    {
        get
        {
            return this.SignalDefinition?.IsSynchronous ?? false;
        }
    }

    public ISignalDefinition SignalDefinition { get; set; } = null;

    public string[] Sections { get; set; } = new string[0];
    public List<string> OtherSections { get; set; } = new List<string>();

    public int HandlerId { get; set; } = 0;

    public MessageTopic()
    {
        
    }

    public MessageTopic(string tenant, string workspace, string module, string signal, string entity = null, string entityId = null, string field = null)
    {
        this.Tenant = tenant;
        this.Workspace = workspace;
        this.Module = module;
        this.Signal = signal;
        this.Entity = entity;
        this.EntityId = entityId;
        this.Field = field;
        this.Check();
    }

    public object Clone()
    {

        return new MessageTopic
        {
            Tenant = this.Tenant,
            Workspace = this.Workspace,
            Module = this.Module,
            Signal = this.Signal,
            Entity = this.Entity,
            EntityId = this.EntityId,
            Field = this.Field,
            IsSystemLevel = this.IsSystemLevel,
            SignalDefinition = this.SignalDefinition,
            Sections = (string[])this.Sections.Clone(),
            OtherSections = new List<string>(this.OtherSections)
        };
    }

    public int CompareTo(MessageTopic? other)
    {
        return string.Compare(this.ToString(), other?.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public bool Equals(MessageTopic? other)
    {
        if (other == null)
            return false;
        return this.ToString().Equals(other.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    protected void CheckSections()
    {
        if (this.Sections.IsNullOrEmpty())
        {
            List<string> secs = new List<string>
            {
                this.Tenant,
                this.Workspace,
                this.Module,
                this.Signal
            };

            if (this.Entity.IsNOTNullOrEmpty())
            {
                secs.Add(this.Entity);
            }

            if (this.EntityId.IsNOTNullOrEmpty())
            {
                secs.Add(this.EntityId);
            }

            if (this.Field.IsNOTNullOrEmpty())
            {
                secs.Add(this.Field);
            }

            this.Sections = secs.ToArray();
        }
    }

    protected void Validate()
    {
        if (this.Tenant.IsNullOrEmpty())
            throw new ArgumentException("Tenant is required for MessageTopic");
        if (this.Workspace.IsNullOrEmpty())
            throw new ArgumentException("Workspace is required for MessageTopic");
        if (this.Module.IsNullOrEmpty())
            throw new ArgumentException("Module is required for MessageTopic");
        if (this.Signal.IsNullOrEmpty())
            throw new ArgumentException("Signal is required for MessageTopic");
    }

    public MessageTopic Check()
    {
        this.Validate();
        this.CheckSections();

        return this;
    }

    public override string ToString()
    {
        string topic = $"{Tenant}/{Workspace}/{Module}/{Signal}";
        if (Entity.IsNOTNullOrEmpty())
        {
            topic += $"/{Entity}";
        }

        if (EntityId.IsNOTNullOrEmpty())
        {
            topic += $"/{EntityId}";
        }

        if (Field.IsNOTNullOrEmpty())
        {
            topic += $"/{Field}";
        }

        if (this.OtherSections.Any())
        {
            topic += $"/{string.Join("/", this.OtherSections)}";
        }

        return topic;
    }

    public static implicit operator string(MessageTopic topic)
    {
        return topic.ToString();
    }

    public static implicit operator MessageTopic(string topicText)
    {
        TopicParser.TopicParseResult result = TopicParser.Parse(Rapidex.Common.MessageHub, topicText);
        if (result.Valid)
        {
            return result.Topic;
        }
        else
        {
            throw new ValidationException($"Invalid topic: {topicText}");
        }
    }

    public static MessageTopic CreateForEntity(string signalName, string entityName)
    {
        return new MessageTopic
        {
            Tenant = ANY,
            Workspace = ANY,
            Module = ANY,
            Signal = signalName,
            Entity = entityName,
            EntityId = ANY,
        };

    }
}
