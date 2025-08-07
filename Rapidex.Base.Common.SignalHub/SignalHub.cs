using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rapidex.SignalHub;
internal class SignalHub : ISignalHub
{
    public ISignalDefinitionCollection Definitions { get; } = new SignalDefinitionCollection();

    int lastId = 1000000;

    internal SignalHubSubscriptionTree Subscriptions { get; } = new SignalHubSubscriptionTree();


    protected int GetId()
    {
        Interlocked.Increment(ref this.lastId);
        return this.lastId;
    }

    protected ISignalArguments Invoke(SignalHubSubscriber subscriber, ISignalArguments args)
    {
        ISignalArguments resultArgs = subscriber.Handler.Invoke(args);
        return resultArgs;
    }

    protected IResult<IEnumerable<ISignalArguments>> PublishInternalSync(SignalTopic topic, ISignalArguments args)
    {
        args.NotNull();
        args.Topic.NotNull();

        SignalHubSubscriber[] subscribers = this.Subscriptions.GetSubscribers(topic.Sections.ToArray());
        if (subscribers.IsNullOrEmpty())
            return Result<IEnumerable<ISignalArguments>>.Ok(new List<ISignalArguments>());

        ISignalArguments _input = args.Clone<ISignalArguments>();

        List<ISignalArguments> returnedArgs = new List<ISignalArguments>();

        foreach (SignalHubSubscriber subs in subscribers)
            try
            {
                ISignalArguments argsForInvoke = _input;
                argsForInvoke.ClientId = subs.ClientId;
                argsForInvoke.HandlerId = subs.Id;

                ISignalArguments resultArg = this.Invoke(subs, argsForInvoke);
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
        Result<IEnumerable<ISignalArguments>> result = Result<IEnumerable<ISignalArguments>>.Ok(returnedArgs.AsReadOnly());
        return result;
    }

    protected virtual IResult<IEnumerable<ISignalArguments>> PublishInternal(SignalTopic topic, ISignalArguments args)
    {
        if (topic.SignalDefinition != null)
        {
            topic.SignalDefinition = this.Definitions.Get(topic.Signal);
        }

        topic.Check()
            .Sections.NotEmpty();

        args.Topic = topic;

        SignalHubSubscriber[] subscribers = this.Subscriptions.GetSubscribers(topic.Sections.ToArray());
        if (subscribers.IsNullOrEmpty())
            return Result<IEnumerable<ISignalArguments>>.Ok(new List<ISignalArguments>());

        args.Id = Guid.NewGuid();

        return this.PublishInternalSync(topic, args);

        //Job_PublishInternalAsync
    }

    public Task<IResult<IEnumerable<ISignalArguments>>> Publish(SignalTopic topic, ISignalArguments args)
    {
        return Task<IResult<IEnumerable<ISignalArguments>>>.Run(() =>
        {
            return this.PublishInternal(topic, args);
        });
    }

    public async Task<IResult<int>> Subscribe(string clientId, SignalTopic topic, Func<ISignalArguments, ISignalArguments> handler)
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
