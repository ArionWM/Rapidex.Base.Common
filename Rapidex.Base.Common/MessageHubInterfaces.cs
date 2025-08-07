using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex;

public interface ISignalDefinition
{
    string SignalName { get; }
    string Description { get; }
    string Category { get; }

    bool IsEntityReleated { get; } //Is the signal related to an entity. If true, the signal will be raised with an entity id and name. If false, the signal will be raised without an entity id and name.
    bool IsSynchronous { get; }

    //string ArgumentTypeName { get; } //Type name of the argument type. This is used to create the argument object. It should be a class that implements ISignalArguments. This is used to create the argument object. It should be a class that implements ISignalArguments.

    //Can ui access ?
}

public interface ISignalDefinitionCollection : IDictionary<string, ISignalDefinition>
{
    void Add(ISignalDefinition def);
    ISignalDefinition Find(string signal);
}


public interface ISignalArguments : ICloneable
{
    SignalTopic Topic { get; set; } 
    Guid Id { get; set; } //Unique id of the message. This is used to identify the message in the system. It should be a guid.
    string ClientId { get; set; }
    int HandlerId { get; set; }
    string SignalName { get; set; }
    bool IsSynchronous { get; set; }
    string Tags { get; }
    string Data { get; }
    string ContentType { get; }
}


public interface ISignalHub
{
    ISignalDefinitionCollection Definitions { get; }


    void Start(IServiceProvider serviceProvider);

    /// <summary>
    ///     Subscribe to a signal.
    /// </summary>
    /// <param name="topic">The signal to subscribe to.</param>
    /// <param name="handler">The handler to call when the signal is raised.</param>
    Task<IResult<int>> Subscribe(string clientId, SignalTopic topic, Func<ISignalArguments, ISignalArguments> handler);

    /// <summary>
    ///     Unsubscribe from a signal.
    /// </summary>
    /// <param name="topic">The signal to unsubscribe from.</param>
    /// <param name="handler">The handler to remove.</param>
    Task<IResult> Unsubscribe(int handlerId);

    /// <summary>
    ///     Raise a signal.
    /// </summary>
    /// <param name="topic">The signal to raise.</param>
    Task<IResult<IEnumerable<ISignalArguments>>> Publish(SignalTopic topic, ISignalArguments args);

    void RegisterSignalDefinition(ISignalDefinition signalDefinition);
}
