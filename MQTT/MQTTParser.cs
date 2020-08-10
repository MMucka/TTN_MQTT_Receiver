using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using MQTTCloud.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTCloud.MQTT
{
    public class MQTTParser
    {
        public static string DecodeBase64(string payload)
        {
            byte[] data = Convert.FromBase64String(payload);
            return Encoding.UTF8.GetString(data);
        }

        public static Message ParseMessage(string payload)
        {
            try
            {
                Message msg = new Message();
                msg.Gateways = new List<Gateway>();

                JObject payloadJSON = JObject.Parse(payload);
                JObject metadata = payloadJSON.SelectToken("metadata").Value<JObject>();
                JArray gateways = metadata.SelectToken("gateways").Value<JArray>();

                msg.DevId = payloadJSON.SelectToken("dev_id").Value<string>();
                string payloadRaw = payloadJSON.SelectToken("payload_raw").Value<string>();
                msg.Airtime = metadata.SelectToken("airtime").Value<long>();

                string time = metadata.SelectToken("time").Value<string>();
                if (DateTime.TryParse(time, out var parTime))
                {
                    Console.WriteLine($"New message {parTime}");
                    msg.Time = parTime;
                }

                try
                {
                    //"{\"packet\": 69, \"gps\": {\"lat\": 0.00, \"lng\": 0.00}"
                    JObject data = JObject.Parse(DecodeBase64(DecodeBase64(payloadRaw)));
                    msg.Latitude = data.SelectToken("lat").Value<float>();
                    msg.Longitude = data.SelectToken("lng").Value<float>();
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                for (var obj = gateways.First; obj != null; obj = gateways.Next)
                {
                    Gateway gtw = new Gateway
                    {
                        GtwId = obj.SelectToken("gtw_id").Value<string>(),
                        Timestamp = obj.SelectToken("timestamp").Value<long>(),
                        Rssi = obj.SelectToken("rssi").Value<int>(),
                        Snr = obj.SelectToken("snr").Value<float>(),
                        Latitude = obj.SelectToken("latitude").Value<float>(),
                        Longitude = obj.SelectToken("longitude").Value<float>()
                    };
                    msg.Gateways.Add(gtw);
                }

                return msg;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"#### Error parsing JSON message: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"#### Error : {ex.Message}");
                Console.WriteLine(payload);
                return null;
            }
        }

        public static string ParseActivation(string data)
        {
            return data;
        }
    }
}
