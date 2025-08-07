//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;

//namespace Rapidex.MessageHub.Mqtt;
//internal class AsynchronousMessageHub : IAsynchronousMessageHub
//{
//    int lastId = 1000000;
//    protected MqttServerConfiguration Options { get; set; }
//    protected MessageHubEmbeddedMqttServer Server { get; set; }
//    protected MessageHubMqttClient Client { get; set; }

//    protected Dictionary<int, MessageTopic> Subscriptions { get; } = new Dictionary<int, MessageTopic>();

//    public AsynchronousMessageHub(IOptions<MqttServerConfiguration> options)
//    {
//        Options = options.Value;
//    }


//    protected int GetId()
//    {
//        Interlocked.Increment(ref this.lastId);
//        return this.lastId;
//    }

//    public void Start(IServiceProvider serviceProvider)
//    {
//        Log.Info($"AsynchronousMessageHub; starting");
//        Log.Info($"{this.Options.ToJson()}");

//        if (this.Options.ServerEnabled)
//        {
//            Log.Info($"AsynchronousMessageHub; creating server");
//            this.Server = TypeHelper.CreateInstanceWithDI<MessageHubEmbeddedMqttServer>();
//            this.Server.Start(serviceProvider);
//        }
//        else
//        {
//            Log.Info($"AsynchronousMessageHub; server not required");
//        }

//        this.Client = TypeHelper.CreateInstanceWithDI<MessageHubMqttClient>();
//        this.Client.Start(serviceProvider);
//    }

//    public async Task<IResult> Publish(MessageTopic topic, IMessageArguments args)
//    {
//        return await this.Client.Publish(topic, args);
//    }

//    public async Task<IResult<int>> Subscribe(MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler)
//    {
//        IResult result = await this.Client.Subscribe(topic, handler);
//        if (result.Success)
//        {
//            int id = this.GetId();
//            this.Subscriptions.Add(id, topic);
//            return Result<int>.Ok(id);
//        }
//        else
//        {
//            return Result<int>.Failure(result.Description);
//        }
//    }

//    public async Task<IResult> Unsubscribe(int handlerId)
//    {
//        MessageTopic topic = this.Subscriptions.Get(handlerId);
//        if (topic == null)
//            return Result.Failure($"HandlerId {handlerId} not found");

//        IResult result = await this.Client.Unsubscribe(topic);

//        if (result.Success)
//        {
//            this.Subscriptions.Remove(handlerId);
//            return result;
//        }
//        else
//        {
//            return result;
//        }
//    }
//}
