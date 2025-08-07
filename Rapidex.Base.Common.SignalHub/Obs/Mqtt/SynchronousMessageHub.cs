//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Rapidex.MessageHub;
//internal class SynchronousMessageHub : ISynchronousMessageHub
//{
//    int lastId = 1000000;

//    internal MessageHubSubscriptionTree Subscriptions { get; } = new MessageHubSubscriptionTree();


//    protected int GetId()
//    {
//        Interlocked.Increment(ref this.lastId);
//        return this.lastId;
//    }

//    protected IMessageArguments Invoke(MessageHubSubscriber subscriber, IMessageArguments args)
//    {
//        IMessageArguments resultArgs = subscriber.Handler.Invoke(args);
//        return resultArgs;
//    }

//    protected IResult PublishInternal(MessageTopic topic, IMessageArguments args)
//    {
//        MessageHubSubscriber[] subscribers = this.Subscriptions.GetSubscribers(topic.Sections.ToArray());
//        if (subscribers.IsNullOrEmpty())
//            return Result.Ok();

//        foreach (MessageHubSubscriber subs in subscribers)
//            try
//            {
//                IMessageArguments result = this.Invoke(subs, args);
//            }
//            catch (Exception ex)
//            {
//                ex.Log();
//            }

//        return Result.Ok();
//    }

//    public Task<IResult> Publish(MessageTopic topic, IMessageArguments args)
//    {
//        return Task<IResult>.Run(() =>
//        {
//            return this.PublishInternal(topic, args);
//        });
//    }

//    public Task<IResult<int>> Subscribe(MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler)
//    {

//        var subscriber = new MessageHubSubscriber(this.GetId(), topic, handler);
//        Subscriptions.Add(subscriber);
//        return Task<IResult<int>>.FromResult((IResult<int>)Result<int>.Ok(subscriber.Id));

//    }

//    public Task<IResult> Unsubscribe(int handlerId)
//    {
//        throw new NotImplementedException();
//    }

//    public void Start(IServiceProvider serviceProvider)
//    {
        
//    }
//}
