//using Rapidex.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Rapidex.SignalHub;
//internal class TopicParser
//{
//    private static readonly Regex MqttTopicRegex = new Regex(
//        @"^(\/?([a-zA-Z0-9_\-]+|(\+)))*(\/(\#))?$",
//        RegexOptions.Compiled | RegexOptions.CultureInvariant
//    );



//    public struct TopicParseResult
//    {
//        public bool Valid { get; set; }
//        public bool Match { get; set; }
//        public MessageTopic Topic { get; set; }

//        public string Description { get; set; }

//        public static TopicParseResult Invalid(string desc)
//        {
//            return new TopicParseResult
//            {
//                Valid = false,
//                Match = false,
//                Topic = null,
//                Description = desc
//            };
//        }

//        public static TopicParseResult Unmatched(string desc)
//        {
//            return new TopicParseResult
//            {
//                Valid = true,
//                Match = false,
//                Topic = null,
//                Description = desc
//            };
//        }
//    }

//    public static TopicParseResult Parse(IMessageHub hub, string topicText)
//    {

//        if (!MqttTopicRegex.IsMatch(topicText))
//        {
//            return TopicParseResult.Invalid("Topic not match regular structure");
//        }

//        //<tenantShortName>/<workspace>/<module>/<signal>/<signal type specific>
//        List<string> parts = topicText.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
//        if (parts.Count < 4)
//        {
//            return TopicParseResult.Invalid("Topic must have 4 section (min)");
//        }

//        MessageTopic topic = new MessageTopic();
//        topic.Sections = parts.ToArray();

//        topic.Tenant = parts[0];
//        parts.RemoveAt(0);

//        if (SignalConstants.WildcardStrs.Contains(topic.Tenant))
//        {
//            topic.IsSystemLevel = true;
//        }

//        //Different tenant is system level

//        topic.Workspace = parts[0];
//        parts.RemoveAt(0);

//        topic.Module = parts[0];
//        parts.RemoveAt(0);

//        topic.Signal = parts[0];
//        parts.RemoveAt(0);

//        if (SignalConstants.WildcardStrs.Contains(topic.Signal))
//        {
//            return TopicParseResult.Invalid($"'Signal' section can't have any wildcard ('{topic.Signal}')");
//        }

//        ISignalDefinition sdef = hub?.Definitions.Find(topic.Signal);
//        bool isMatch = sdef != null;

//        topic.SignalDefinition = sdef;

//        if (sdef != null && sdef.IsEntityReleated)
//        {
//            if (parts.Count > 0)
//            {
//                topic.Entity = parts[0];
//                parts.RemoveAt(0);
//            }

//            if (parts.Count > 0)
//            {
//                topic.EntityId = parts[0];
//                parts.RemoveAt(0);
//            }

//            if (parts.Count > 0)
//            {
//                topic.Field = parts[0];
//                parts.RemoveAt(0);
//            }
//        }

//        if (parts.Count > 0)
//        {
//            topic.OtherSections.AddRange(parts);
//        }

//        return new TopicParseResult
//        {
//            Match = isMatch,
//            Valid = true,
//            Topic = topic
//        };

//    }
//}
