using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using MQTTCloud.Models;
using MQTTCloud.Services;
using MQTTnet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTCloud.MQTT
{
    public class MQTTParser
    {
        private readonly MessagesService _messagesService;
        private readonly GatewaysService _gatewaysService;
        private readonly DevicesService _devicesService;

        public MQTTParser(MessagesService messagesService, GatewaysService gatewaysService, DevicesService devicesService)
        {
            _messagesService = messagesService;
            _gatewaysService = gatewaysService;
            _devicesService = devicesService;
        }

        public void ReceivedMessage(MqttApplicationMessageReceivedEventArgs receivedData)
        {
            if (receivedData.ProcessingFailed)
            {
                Console.WriteLine($"#### Error processing message from client {receivedData.ClientId}");
                return;
            }

            try
            {
                string message = Encoding.UTF8.GetString(receivedData.ApplicationMessage.Payload);
                string topic = receivedData.ApplicationMessage.Topic;
                string[] topicTokens = topic.Split('/');    
                
                if(topicTokens.Last() == "up")    //lora_tracker_id/devices/node_esp32/up     --From device
                {
                    JObject messageJson = JObject.Parse(message);
                    var msg = ParseMessage(messageJson);
                    if (msg != null)        //parsed message
                    {
                        Message savedMessage = _messagesService.Add(msg);
                        ParseGateways(messageJson, savedMessage);
                    }
                }
                
                else if (topicTokens.Last() == "activations")  //lora_tracker_id/devices/node_esp32/events/activations      --Joined
                {
                    ParseActivation(message);
                    Console.WriteLine("Device Joined: " + topicTokens[2]);
                }
                
                else                             //lora_tracker_id/devices/node_esp32/events/down/sent        --To device
                {
                    Console.WriteLine("Unknown topic: " + receivedData.ApplicationMessage.Topic);
                    Console.WriteLine(message);
                }
                
            }
            catch (DecoderFallbackException ex)
            {
                Console.WriteLine($"ReceivedMessage: Error decode message to UTF-8: {ex.Message}");
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"ReceivedMessage: Error parsing JSON message: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ReceivedMessage: {ex.Message}");
            }

        }

        private Message ParseMessage(JObject messageJson)
        {
            Message msg = new Message();
            JObject metadata = (messageJson.SelectToken("metadata") ?? throw new JsonReaderException()).Value<JObject>();

            msg.DevId = messageJson.SelectToken("dev_id")!.Value<string>();
            msg.Airtime = metadata.SelectToken("airtime")!.Value<long>();
            Device dev = _devicesService.FindDevId(msg.DevId);
            msg.DeviceId = dev.Id;
            
            string time = metadata.SelectToken("time")!.Value<string>();
            if (DateTime.TryParse(time, out var parTime))
            {
                Console.WriteLine($"New message {parTime}");
                msg.Time = parTime;
            }

            byte[] nullIv = new byte[16];
            string payloadRaw = messageJson.SelectToken("payload_raw")!.Value<string>();
            byte[] payloadEncrypted = Convert.FromBase64String(payloadRaw);    //decode Base64
            //string payloadDecrypted = AESCryptography.DecryptStringFromBytes_Aes(    //decode AES
            //    payloadEncrypted, dev.AesKey, nullIv);
            
            Console.WriteLine(payloadEncrypted);

            int lat = payloadEncrypted[0];
            int latD = (payloadEncrypted[1] << 8) + payloadEncrypted[2];
            int lng = payloadEncrypted[3];
            int lngD = (payloadEncrypted[4] << 8) + payloadEncrypted[5];

            float latF = lat + (latD / 10000f);
            float lngF = lng + (lngD / 10000f);
            
            Console.WriteLine(latF);
            Console.WriteLine(lngF);

            msg.Latitude = latF;
            msg.Longitude = lngF;
            
            //(Encoding.UTF8.GetString(byte[]);
            //"{\"packet\": 69, \"gps\": {\"lat\": 0.00, \"lng\": 0.00}}"
            /*JObject dataJ = JObject.Parse(payloadDecrypted);
            JObject gpsData = dataJ.SelectToken("gps")!.Value<JObject>();
            msg.Latitude = gpsData.SelectToken("lat")!.Value<float>();
            msg.Longitude = gpsData.SelectToken("lng")!.Value<float>();*/

            return msg;
        }

        private void ParseGateways(JObject messageJson, Message msg)
        {
            JObject metadata = messageJson.SelectToken("metadata")!.Value<JObject>();
            JArray gateways = metadata.SelectToken("gateways")!.Value<JArray>();
            for (var obj = gateways.First; obj != null; obj = gateways.Next)
            {
                Gateway gtw =new Gateway(){
                    GtwId = obj.SelectToken("gtw_id")!.Value<string>(),
                    MessageId = msg.Id,
                    Timestamp = obj.SelectToken("timestamp")!.Value<long>(),
                    Rssi = obj.SelectToken("rssi")!.Value<int>(),
                    Snr = obj.SelectToken("snr")!.Value<float>(),
                    Latitude = obj.SelectToken("latitude")!.Value<float>(),
                    Longitude = obj.SelectToken("longitude")!.Value<float>()
                };
                _gatewaysService.Add(gtw);
            }
        }

        private static string ParseActivation(string data)
        {
            return data;
        }
    }
}
