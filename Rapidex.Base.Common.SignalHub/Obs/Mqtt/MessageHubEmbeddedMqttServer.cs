//using Microsoft.Extensions.Options;
//using MQTTnet.Server;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rapidex.MessageHub;
//internal class MessageHubEmbeddedMqttServer : IManager
//{
//    protected MqttServer Server { get; set; }

//    protected MqttServerConfiguration Options { get; set; }

//    public MessageHubEmbeddedMqttServer(IOptions<MqttServerConfiguration> options)
//    {
//        this.Options = options.Value;
//    }

//    public MessageHubEmbeddedMqttServer(MqttServerConfiguration config)
//    {
//        this.Options = config;

//    }

//    protected MqttServerOptions CreateOptions()
//    {
//        // Setup client validator.
//        var optionsBuilder = new MqttServerOptionsBuilder();

//        MqttServerOptions opt = optionsBuilder
//            .WithDefaultEndpoint()
//            .Build();
//        //opt.EnablePersistentSessions = true;
//        return opt;

//            //.WithConnectionValidator(c =>
//            //{
//            //    if (c.ClientId.Length < 10)
//            //    {
//            //        c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
//            //        return;
//            //    }

//        //    if (c.Username != "mySecretUser")
//        //    {
//        //        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
//        //        return;
//        //    }

//        //    if (c.Password != "mySecretPassword")
//        //    {
//        //        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
//        //        return;
//        //    }

//        //    c.ReasonCode = MqttConnectReasonCode.Success;
//        //});
//    }

//    public void Start(IServiceProvider serviceProvider)
//    {
//        //https://github.com/dotnet/MQTTnet/wiki/Server#preparation

//        MQTTnet.Server.MqttServerFactory mqttServerFactory = new MQTTnet.Server.MqttServerFactory();
//        MqttServerOptions opt = this.CreateOptions();
        

//        this.Server = mqttServerFactory.CreateMqttServer(opt);
//        this.Server.StartAsync();
//    }

//    public void Setup(IServiceCollection services)
//    {
//        throw new NotSupportedException();
//    }
//}
