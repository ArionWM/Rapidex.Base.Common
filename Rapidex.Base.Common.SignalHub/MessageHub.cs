using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rapidex.MessageHub;
internal class MessageHub : IMessageHub
{
    public ISignalDefinitionCollection Definitions { get; } = new SignalDefinitionCollection();

    int lastId = 1000000;

    internal MessageHubSubscriptionTree Subscriptions { get; } = new MessageHubSubscriptionTree();


    protected int GetId()
    {
        Interlocked.Increment(ref this.lastId);
        return this.lastId;
    }

    protected IMessageArguments Invoke(MessageHubSubscriber subscriber, IMessageArguments args)
    {
        IMessageArguments resultArgs = subscriber.Handler.Invoke(args);
        return resultArgs;
    }

    protected IResult<IEnumerable<IMessageArguments>> PublishInternalSync(MessageTopic topic, IMessageArguments args)
    {
        args.NotNull();
        args.Topic.NotNull();

        MessageHubSubscriber[] subscribers = this.Subscriptions.GetSubscribers(topic.Sections.ToArray());
        if (subscribers.IsNullOrEmpty())
            return Result<IEnumerable<IMessageArguments>>.Ok(new List<IMessageArguments>());

        IMessageArguments _input = args.Clone<IMessageArguments>();

        List<IMessageArguments> returnedArgs = new List<IMessageArguments>();

        foreach (MessageHubSubscriber subs in subscribers)
            try
            {
                IMessageArguments argsForInvoke = _input;
                argsForInvoke.ClientId = subs.ClientId;
                argsForInvoke.HandlerId = subs.Id;

                IMessageArguments resultArg = this.Invoke(subs, argsForInvoke);
                if (resultArg == null)
                {
                    returnedArgs.Add(argsForInvoke);
                }
                else
                {
                    //Eğer publish olan değişiklik yaptı ise diğerine veriyoruz.
                    _input = resultArg;
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }

        //Sonuncu resultArg 'ı alıyoruz.
        returnedArgs.Insert(0, _input);
        Result<IEnumerable<IMessageArguments>> result = Result<IEnumerable<IMessageArguments>>.Ok(returnedArgs.AsReadOnly());
        return result;
    }

    protected virtual IResult<IEnumerable<IMessageArguments>> PublishInternal(MessageTopic topic, IMessageArguments args)
    {
        if (topic.SignalDefinition != null)
        {
            topic.SignalDefinition = this.Definitions.Get(topic.Signal);
        }

        topic.Check()
            .Sections.NotEmpty();

        args.Topic = topic;

        MessageHubSubscriber[] subscribers = this.Subscriptions.GetSubscribers(topic.Sections.ToArray());
        if (subscribers.IsNullOrEmpty())
            return Result<IEnumerable<IMessageArguments>>.Ok(new List<IMessageArguments>());

        args.Id = Guid.NewGuid();

        return this.PublishInternalSync(topic, args);

        //Job_PublishInternalAsync
    }

    public Task<IResult<IEnumerable<IMessageArguments>>> Publish(MessageTopic topic, IMessageArguments args)
    {
        return Task<IResult<IEnumerable<IMessageArguments>>>.Run(() =>
        {
            return this.PublishInternal(topic, args);
        });
    }

    public async Task<IResult<int>> Subscribe(string clientId, MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler)
    {
        topic.Check()
            .Sections.NotEmpty();

        int handlerID = this.GetId();
        this.Subscriptions.Add(clientId, handlerID, topic, handler);
        return await Task<IResult<int>>.FromResult((IResult<int>)Result<int>.Ok(handlerID));

    }

    public async Task<IResult> Unsubscribe(int handlerId)
    {
        this.Subscriptions.Remove(handlerId);
        return await Task<IResult>.FromResult(Result.Ok());
    }

    public void RegisterSignalDefinition(ISignalDefinition signalDefinition)
    {
        this.Definitions.Set(signalDefinition.SignalName, signalDefinition);
    }

    public void Start(IServiceProvider serviceProvider)
    {

    }
}
