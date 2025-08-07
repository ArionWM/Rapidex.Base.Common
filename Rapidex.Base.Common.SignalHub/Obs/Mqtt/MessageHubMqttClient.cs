//using Microsoft.Extensions.Options;
//using MQTTnet;
//using MQTTnet.Formatter;
//using MQTTnet.Packets;
//using MQTTnet.Protocol;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Rapidex.MessageHub.Mqtt;
//internal class MessageHubMqttClient : IManager
//{
//    protected MqttClientFactory MqttClientFactory { get; set; }
//    protected MqttServerConfiguration Options { get; set; }
//    protected IMqttClient MqttClient { get; set; }

//    public MessageHubMqttClient(IOptions<MqttServerConfiguration> options)
//    {
//        this.Options = options.Value;
//    }

//    public MessageHubMqttClient(MqttServerConfiguration config)
//    {
//        this.Options = config;
//    }

//    protected MqttClientOptions CreateOptions()
//    {
//        string host = this.Options.Host ?? "localhost";

//        var options = new MqttClientOptionsBuilder()
//            .WithTcpServer("localhost")
//            .WithProtocolVersion(MqttProtocolVersion.V500)
//            //.WithClientId("Client1")
//            //.WithTcpServer("broker.hivemq.com")
//            //.WithCredentials("bud", "%spencer%")
//            //.WithEndPoint("broker.hivemq.com", 1883)
//            //.WithTls()
//            //.WithCleanSession()
//            .Build();

//        return options;
//    }

//    public void Setup(IServiceCollection services)
//    {

//        throw new NotSupportedException();
//    }

//    public void Start(IServiceProvider serviceProvider)
//    {
//        var options = CreateOptions();
//        this.MqttClientFactory = new MqttClientFactory();
//        this.MqttClient = this.MqttClientFactory.CreateMqttClient();
//        this.MqttClient.ConnectAsync(options);
//    }

//    /*
//  var applicationMessage = new MqttApplicationMessageBuilder()
//                .WithTopic("samples/temperature/living_room")
//                .WithPayload("19.5")
//                .Build();

//            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);     
//     */

//    public async Task<IResult> Publish(MessageTopic topic, IMessageArguments args)
//    {
//        var applicationMessage = new MqttApplicationMessageBuilder()
//            .WithTopic(topic.ToString())
//            .WithPayload(args.ToJson())
//            .Build();

//        var res = this.MqttClient.PublishAsync(applicationMessage)
//              .ContinueWith<IResult>(mqttRes =>
//              {
//                  if (mqttRes.IsFaulted)
//                  {
//                      return Result.Failure(mqttRes.Exception.ToString());
//                  }
//                  else
//                  {
//                      return Result.Ok();
//                  }
//              });

//        return await res;
//    }

//    public async Task<IResult> Subscribe(MessageTopic topic, Func<IMessageArguments, IMessageArguments> handler)
//    {
//        var mqttSubscribeOptions = this.MqttClientFactory
//            .CreateSubscribeOptionsBuilder()
//            .WithTopicFilter(
//              new MqttTopicFilter
//              {
//                  Topic = topic.ToString(),
//                  QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce
//              })
//            .Build();

//        var response = this.MqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

//        return await response.ContinueWith<IResult>(mqttRes =>
//        {
//            if (mqttRes.IsFaulted)
//            {
//                return Result<int>.Failure(mqttRes.Exception.ToString());
//            }
//            else
//            {
//                //mqttRes.Result
//                return Result.Ok();
//            }
//        });
//    }

//    public Task<IResult> Unsubscribe(MessageTopic topic)
//    {
//        var mqttUnsubscribeOptions = this.MqttClientFactory
//            .CreateUnsubscribeOptionsBuilder()
//            .WithTopicFilter(topic.ToString())
//            .Build();
//        var response = this.MqttClient.UnsubscribeAsync(mqttUnsubscribeOptions, CancellationToken.None);
//        return response.ContinueWith<IResult>(mqttRes =>
//        {
//            if (mqttRes.IsFaulted)
//            {
//                return Result.Failure(mqttRes.Exception.ToString());
//            }
//            else
//            {
//                return Result.Ok();
//            }
//        });

//    }
//}
