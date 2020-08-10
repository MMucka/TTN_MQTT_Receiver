using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MQTTCloud.Models;
using MQTTCloud.MQTT;

namespace MQTTCloud.Services
{
    public class DevicesService : BaseService<Device>
    {
        private readonly BlockingDevices _blockingDevices;
        private readonly ApplicationsService _applicationsService;

        public DevicesService (IServiceProvider provider, IConfiguration config, BlockingDevices blockingDevices, ApplicationsService applicationsService) : base(provider, config)
        {
            _blockingDevices = blockingDevices;
            _applicationsService = applicationsService;
        }

        public override Device Add(Device dev)
        {
            var context = CreateDbContext(new string[]{});
            dev.AesKey = AESCryptography.GenerateKey();
            dev.AesIv = AESCryptography.GenerateIV();
            
            context.Add(dev);
            context.SaveChanges();

            _blockingDevices.Add(dev);
            return dev;
        }
        
        public Device FindDevId(string devId)
        {
            var context = CreateDbContext(new string[]{});
            return context.Devices.First(d => d.DevID == devId);
        }
        
        public Device ActivateDevice(long id)
        {
            using var context = CreateDbContext(new string[] { });
            Device dev = context.Find<Device>(id);
            dev.Active = true;
            context.SaveChanges();
            
            _blockingDevices.Add(dev);
            return dev;
        }
        
        public Device DeactivateDevice(long id)
        {
            using var context = CreateDbContext(new string[] { });
            Device dev = context.Find<Device>(id);
            dev.Active = false;
            context.SaveChanges();
            
            _blockingDevices.Add(dev);
            return dev;
        }
        
        public List<Device> GetAppDevices(long id)
        {
            var context = CreateDbContext(new string[] { });
            var devs = context.Devices.Where(d => d.ApplicationId == id).ToList();
            return devs;
        }

        public override Device Delete(long id)
        {
            Device dev = base.Delete(id);
            if (dev.Active)
            {
                dev.Active = false;
                _blockingDevices.Add(dev);
            }

            if (GetAppDevices(dev.ApplicationId).Count == 0)
            {
                _applicationsService.Delete(dev.ApplicationId);
            }

            return dev;
        }
    }
}