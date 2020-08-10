using MQTTCloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace MQTTCloud.Services
{
    public class MessagesService : BaseService<Message>
    {
        private readonly DevicesService _devicesService;
        private readonly GatewaysService _gatewaysService;

        public MessagesService (IServiceProvider provider, IConfiguration config, 
            DevicesService devicesService, GatewaysService gatewaysService) : base(provider, config)
        {
            _devicesService = devicesService;
            _gatewaysService = gatewaysService;
        }

        public override Message Delete(long id)
        {
            _gatewaysService.DeleteMessageGateways(id);
            return base.Delete(id);
        }

        public string ListDeviceMessages(long deviceId)
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                //var message = context.Messages.Where(m => m.DeviceId == deviceId)
                //    .OrderByDescending(msg => msg.Time).ToList();
                var message = context.Messages.Where(m => m.DeviceId == deviceId).
                    SelectMany(msg => context.Gateways.Where(gtw => gtw.MessageId == msg.Id).
                        DefaultIfEmpty().Select(a => new
                    {
                        msg.Id,
                        msg.DevId,
                        msg.DeviceId,
                        msg.Time,
                        msg.Airtime,
                        a.Rssi,
                        a.Snr,
                        Latitude = Math.Abs(msg.Latitude) < 0.1f ? a.Latitude : msg.Latitude,
                        Longitude = Math.Abs(msg.Longitude) < 0.1f ? a.Longitude : msg.Longitude,
                    })).ToList();

                JArray array = new JArray();
                foreach(var m in message)
                {
                    JObject o = new JObject(
                        new JProperty("Id", m.Id),
                        new JProperty("DevId", m.DevId),
                        new JProperty("DeviceId", m.DeviceId),
                        new JProperty("Time", m.Time),
                        new JProperty("Airtime", m.Airtime),
                        new JProperty("Rssi", m.Rssi),
                        new JProperty("Snr", m.Snr),
                        new JProperty("Latitude", m.Latitude),
                        new JProperty("Longitude", m.Longitude)
                    );
                    array.Add(o);
                }

                return array.ToString();
            }
        }
        
        public new string List()
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Messages.
                    SelectMany(msg => context.Gateways.Where(gtw => gtw.MessageId == msg.Id).
                        DefaultIfEmpty().Select(a => new
                        {
                            msg.Id,
                            msg.DevId,
                            msg.DeviceId,
                            msg.Time,
                            msg.Airtime,
                            a.Rssi,
                            a.Snr,
                            Latitude = Math.Abs(msg.Latitude) < 0.1f ? a.Latitude : msg.Latitude,
                            Longitude = Math.Abs(msg.Longitude) < 0.1f ? a.Longitude : msg.Longitude,
                        })).ToList();

                JArray array = new JArray();
                foreach(var m in message)
                {
                    JObject o = new JObject(
                        new JProperty("Id", m.Id),
                        new JProperty("DevId", m.DevId),
                        new JProperty("DeviceId", m.DeviceId),
                        new JProperty("Time", m.Time),
                        new JProperty("Airtime", m.Airtime),
                        new JProperty("Rssi", m.Rssi),
                        new JProperty("Snr", m.Snr),
                        new JProperty("Latitude", m.Latitude),
                        new JProperty("Longitude", m.Longitude)
                    );
                    array.Add(o);
                }

                return array.ToString();
            }
        }

        public string ListDeviceDatesMessages(long deviceId, DateTime from, DateTime to)
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Messages.Where(m => m.DeviceId == deviceId 
                                                           && m.Time > from && m.Time < to).ToList().
                    SelectMany(msg => context.Gateways.Where(gtw => gtw.MessageId == msg.Id).
                        DefaultIfEmpty().Select(a => new
                        {
                            msg.Id,
                            msg.DevId,
                            msg.DeviceId,
                            msg.Time,
                            msg.Airtime,
                            a.Rssi,
                            a.Snr,
                            Latitude = Math.Abs(msg.Latitude) < 0.1f ? a.Latitude : msg.Latitude,
                            Longitude = Math.Abs(msg.Longitude) < 0.1f ? a.Longitude : msg.Longitude,
                        })).ToList();

                JArray array = new JArray();
                foreach(var m in message)
                {
                    JObject o = new JObject(
                        new JProperty("Id", m.Id),
                        new JProperty("DevId", m.DevId),
                        new JProperty("DeviceId", m.DeviceId),
                        new JProperty("Time", m.Time),
                        new JProperty("Airtime", m.Airtime),
                        new JProperty("Rssi", m.Rssi),
                        new JProperty("Snr", m.Snr),
                        new JProperty("Latitude", m.Latitude),
                        new JProperty("Longitude", m.Longitude)
                    );
                    array.Add(o);
                }

                return array.ToString();
            }
        }
    }
}
