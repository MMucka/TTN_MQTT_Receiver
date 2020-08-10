using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MQTTCloud.Models;

namespace MQTTCloud.Services
{
    public class ApplicationsService : BaseService<Application>
    {

        public ApplicationsService (IServiceProvider provider, IConfiguration config) : base(provider, config)
        {
        }

        public List<Application> GetAppWithDevices()
        {
            var context = CreateDbContext(new string[] { });
            var apps = context.Applications.Include(app => app.Devices).ToList();
            return apps;
        }

        public Application FindName(string appId)
        {
            using var context = CreateDbContext(new string[] { });
            return context.Applications.First(x => x.AppId == appId);
        }
        
        /**
         * Delete all devices with application
         */
        public override Application Delete(long id)
        {
            var context = CreateDbContext(new string[] { });
            
            var appd = context.Applications.Include(app => app.Devices).First(x => x.Id == id);
            if (appd == null)
            {
                throw new Exception("Not found");
            }
            
            foreach(var device in appd.Devices)
            {
                //_devicesService.Delete(device.Id);
            }
            
            context.Set<Application>().Remove(appd);
            context.SaveChanges();

            return appd;
        }
    }
}