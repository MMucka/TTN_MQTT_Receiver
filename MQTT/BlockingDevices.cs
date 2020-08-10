using System.Collections.Concurrent;
using MQTTCloud.Models;

namespace MQTTCloud.MQTT
{
    public class BlockingDevices
    {
        public readonly BlockingCollection<Device> NewDevices = new BlockingCollection<Device>();
        public void Add(Device device)
        {
            NewDevices.Add(device);
        }
    }
}