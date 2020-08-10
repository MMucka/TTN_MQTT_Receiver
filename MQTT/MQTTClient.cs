using System;
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
using MQTTnet.Extensions.ManagedClient;

namespace MQTTCloud
{
    public class MQTTClient : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly string _appId;
        private readonly string _appKey;
        private readonly string _deviceId;
        private readonly MessagesService _messagesService;

        private static IManagedMqttClient _mqttClient;

        public bool Connected { get; private set; } = false;

        public MQTTClient(IConfiguration configuration, MessagesService messagesService)
        {
            _configuration = configuration;
            _appId = configuration.GetConnectionString("AppID");
            _appKey = configuration.GetConnectionString("AppKey");
            _deviceId = configuration.GetConnectionString("DevID");
            _messagesService = messagesService;

            _mqttClient = new MqttFactory().CreateManagedMqttClient();
        }

        /**
         * Connect to MQTT server
         * @param url - address of server
         * @param port - port of MQTT server, default 1883
         */
        public async void ConnectBroker(string url, int port = 1883)
        {
            var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(_appId)
                        .WithTcpServer(url, port)
                        .WithCredentials(_appId, _appKey)
                        .WithCleanSession()
                        .Build())
                    .Build();

            await _mqttClient.StartAsync(options);           //start connecting

            _mqttClient.UseApplicationMessageReceivedHandler(ReceivedMessage);

            _mqttClient.UseConnectedHandler(e =>     //connected, subscribe for topics
            {
                Connected = true;
                SubscribeMqtt(_appId, "node_esp32");
                SubscribeMqtt(_appId, "lora_node_otaa");

                Console.WriteLine("### CONNECTED ###");
            });
        }

        public void SubscribeMqtt(string appId, string devId)
        {
            //lora_tracker_id/devices/lora_node_otaa/up
            _mqttClient.SubscribeAsync(new TopicFilterBuilder()
                    .WithTopic($"{appId}/devices/{devId}/up").Build());                 //new message
            //lora_tracker_id/devices/lora_node_otaa/activations
            _mqttClient.SubscribeAsync(new TopicFilterBuilder()
                    .WithTopic($"{appId}/devices/{devId}/events/activations").Build()); //new device

        }


        private void ReceivedMessage(MqttApplicationMessageReceivedEventArgs receivedData)
        {
            if (receivedData.ProcessingFailed)
            {
                Console.WriteLine($"#### Error processing message from client {receivedData.ClientId}");
                return;
            }

            try
            {
                string message = Encoding.UTF8.GetString(receivedData.ApplicationMessage.Payload);
                if (Regex.Match(receivedData.ApplicationMessage.Topic, ".*\\/up").Success)
                {
                    var msg = MQTTParser.ParseMessage(message);
                    if(msg != null)
                        _messagesService.AddMessage(msg);
                }
                else
                {
                    Console.WriteLine("Unknown topic: ");
                    Console.WriteLine(message);
                }
                
            }
            catch (DecoderFallbackException ex)
            {
                Console.WriteLine($"#### Error decode message to UTF-8: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"#### Error writing do DB: {ex.Message}");
            }

        }

        public void PublishAsyncMessage(string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("lora_tracker_id/devices/lora_node_otaa/down")
                .WithPayload(payload)
                .Build();

            _mqttClient.PublishAsync(message);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                ConnectBroker(_configuration.GetConnectionString("Broker")), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _mqttClient.StopAsync();
        }
    }
}
