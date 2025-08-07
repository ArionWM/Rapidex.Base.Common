using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Rapidex;

public static class YamlHelper
{
    private static YamlDotNet.Serialization.ISerializer _defaultSerializer; // = new YamlDotNet.Serialization.Serializer();
    private static YamlDotNet.Serialization.IDeserializer _defaultDeserializer;

    public static void Setup()
    {
        YamlHelper._defaultSerializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        YamlHelper._defaultDeserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();
    }


    public static string ToYaml<T>(this T obj)
    {
        return _defaultSerializer.Serialize(obj);
    }

    public static T FromYaml<T>(this string yaml)
    {
        return _defaultDeserializer.Deserialize<T>(yaml);

    }

    //https://github.com/aaubry/YamlDotNet/issues/12

    public static IEnumerable<object> DeserializeMany(this IDeserializer deserializer, TextReader input)
    {
        var reader = new Parser(input);
        reader.Consume<StreamStart>();

        DocumentStart dummyStart;
        DocumentEnd dummyEnd;
        while (reader.TryConsume<DocumentStart>(out dummyStart))
        {
            var item = deserializer.Deserialize(reader);
            yield return item;
            reader.TryConsume<DocumentEnd>(out dummyEnd);
        }
    }

    public static IEnumerable<object> DeserializeMany(this IDeserializer deserializer, string input)
    {
        using (var reader = new StringReader(input))
        {
            return deserializer.DeserializeMany(reader).ToArray();
        }
    }

    public static IEnumerable<object> FromYamlMany(this string input)
    {
        using (var reader = new StringReader(input))
        {
            return _defaultDeserializer.DeserializeMany(reader).ToArray();
        }
    }

    public static IEnumerable<TItem> DeserializeMany<TItem>(this IDeserializer deserializer, TextReader input)
    {
        var reader = new Parser(input);
        reader.Consume<StreamStart>();

        DocumentStart dummyStart;
        DocumentEnd dummyEnd;
        while (reader.TryConsume<DocumentStart>(out dummyStart))
        {
            var item = deserializer.Deserialize<TItem>(reader);
            yield return item;
            reader.TryConsume<DocumentEnd>(out dummyEnd);
        }
    }

    public static IEnumerable<TItem> FromYamlMany<TItem>(this string input)
    {
        using (var reader = new StringReader(input))
        {
            return _defaultDeserializer.DeserializeMany<TItem>(reader).ToArray();
        }



    }
}