using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

namespace Rapidex.MessageHub;

//public interface ISignalDefinition
//{
//    string SignalName { get; }
//    string Description { get; }
//    string Category { get; }

//    bool IsEntityReleated { get; } //Is the signal related to an entity. If true, the signal will be raised with an entity id and name. If false, the signal will be raised without an entity id and name.
//    bool IsSynchronous { get; }

//    //string ArgumentTypeName { get; } //Type name of the argument type. This is used to create the argument object. It should be a class that implements ISignalArguments. This is used to create the argument object. It should be a class that implements ISignalArguments.

//    //Can ui access ?
//}

//public interface IMessageArguments : ICloneable
//{
//    Guid Id { get; set; } //Unique id of the message. This is used to identify the message in the system. It should be a guid.
//    string ClientId { get; set; }
//    int HandlerId { get; set; }
//    string SignalName { get; set; }
//    bool IsSynchronous { get; set; }
//    string Tags { get; }
//    string Data { get; }
//    string ContentType { get; }
//}

//public interface IEntityReleatedMessageArguments : IMessageArguments
//{
//    IEntity Entity { get; }

//}

//public interface IMessageHub
//{
//    SignalDefinitionCollection Definitions { get; }


//    void Start(IServiceProvider serviceProvider);

//    /// <summary>
//    ///     Subscribe to a signal.
//    /// </summary>
//    /// <param name="topic">The signal to subscribe to.</param>
//    /// <param name="handler">The handler to call when the signal is raised.</param>
//    Task<IResult<int>> Subscribe(string clientId, MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler);

//    /// <summary>
//    ///     Unsubscribe from a signal.
//    /// </summary>
//    /// <param name="topic">The signal to unsubscribe from.</param>
//    /// <param name="handler">The handler to remove.</param>
//    Task<IResult> Unsubscribe(int handlerId);

//    /// <summary>
//    ///     Raise a signal.
//    /// </summary>
//    /// <param name="topic">The signal to raise.</param>
//    Task<IResult<IEnumerable<IMessageArguments>>> Publish(MessageTopic topic, IMessageArguments args);

//    void RegisterSignalDefinition(ISignalDefinition signalDefinition);
//}


