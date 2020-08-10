using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Newtonsoft.Json.Linq;
using MQTTCloud.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MQTTCloud.MQTT;
using MQTTCloud.Services;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTCloud
{
    public class MQTTClient
    {
        private readonly MQTTParser _parser;
        private static IManagedMqttClient _mqttClient;
        private const string Broker = "eu.thethings.network";
        
        public static bool Connected { get; set; } = false;
        public readonly Application Application;
        
        public MQTTClient(MQTTParser parser, Application application)
        {
            _parser = parser;
            Application = application;
            RunClient(application);
        }
        
        private async void RunClient(Application app)
        {
            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            
            ConnectBroker(app.AppId, app.AppKey, Broker);
            
            _mqttClient.UseApplicationMessageReceivedHandler(_parser.ReceivedMessage);
            _mqttClient.UseConnectedHandler(e =>     //connected, subscribe for topics
            {
                Connected = true;
                Console.WriteLine("Connected to " + app.AppId);
            });
        }
        
        private async void ConnectBroker(string appId, string appKey, string url, int port = 1883)
        {
            var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(appId)
                        .WithTcpServer(url, port)
                        .WithCredentials(appId, appKey)
                        .WithCleanSession()
                        .Build())
                    .Build();

            await _mqttClient.StartAsync(options);           //start connecting
        }

        public void SubscribeMqtt(string devId)
        {
            var appId = Application.AppId;

            string topic = $"{appId}/devices/{devId}/up";                     //lora_tracker_id/devices/lora_node_otaa/up
            _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build()); //new message

            string activated = $"{appId}/devices/{devId}/events/activations"; //lora_tracker_id/devices/lora_node_otaa/activations
            _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(activated).Build()); //new device

        }

        public void UnsubscribeMqtt(string devId)
        {
            var appId = Application.AppId;

            string topic = $"{appId}/devices/{devId}/up";
            string activated = $"{appId}/devices/{devId}/events/activations";
            _mqttClient.UnsubscribeAsync(new string[]{topic, activated});
        }

        public void PublishAsyncMessage(string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("lora_tracker_id/devices/lora_node_otaa/down")
                .WithPayload(payload)
                .Build();

            _mqttClient.PublishAsync(message);
        }

        public void Stop()
        {
            Connected = false;
            _mqttClient.StopAsync();
        }
    }
}
