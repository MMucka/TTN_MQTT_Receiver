using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MQTTCloud.Models;

namespace MQTTCloud.Services
{
    public class GatewaysService : BaseService<Gateway>
    {
        public GatewaysService (IServiceProvider provider, IConfiguration config) : base(provider, config) { }
        
        public List<Gateway> GetMessageGateways(long deviceId)
        {
            using var context = CreateDbContext(new string[] { });
            return context.Gateways.Where(m => m.MessageId == deviceId).ToList();
        }
        
        public void DeleteMessageGateways(long deviceId)
        {
            using var context = CreateDbContext(new string[] { });
            var gateways = context.Gateways.Where(m => m.MessageId == deviceId).ToList();
            foreach (var gtw in gateways)
            {
                context.Remove(gtw);
            }

            context.SaveChanges();
        }
    }
}