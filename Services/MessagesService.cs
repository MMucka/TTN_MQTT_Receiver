using MQTTCloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MQTTCloud.Services
{
    public class MessagesService : IDesignTimeDbContextFactory<MessageContext>
    {
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;

        public MessagesService() { }
        public MessagesService (IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            _config = config;
        }

        public MessageContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessageContext>();
            optionsBuilder.UseNpgsql("User ID =lora;Password=loradbheslo/123;Server=cloud.borntolive.cz;Port=5432;Database=loradb;Integrated Security=true;Pooling=true;");

            return new MessageContext(optionsBuilder.Options);
        }

        public Message AddMessage(Message message)
        {
            var context = CreateDbContext(new string[]{});
            context.Add(message);
            context.SaveChangesAsync();

            return message;
        }

        public Message FindMessage(long id)
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Find<Message>(id);

                return message;
            }
        }

        public Message DeleteMessage(long id)
        {
            var context = CreateDbContext(new string[] { });
            
            var message = context.Find<Message>(id);
            if (message != null)
            {
                context.Messages.Remove(message);
                context.SaveChanges();
            }

            return message;
        }

        public IEnumerable<Message> ListMessages()
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Messages.Include(message1 => message1.Gateways).ToList();

                return message;
            }
        }

        public List<Message> ListDatesMessages(DateTime from, DateTime to)
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Messages.Where(m => m.Time > from && m.Time < to).Include(message1 => message1.Gateways).ToList();

                return message;
            }
        }

        public List<Message> ListDeviceMessages(String devId)
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Messages.Where(m => m.DevId == devId).Include(message1 => message1.Gateways).ToList();

                return message;
            }
        }

        public List<Message> ListDeviceDatesMessages(String devId, DateTime from, DateTime to)
        {
            using (var context = CreateDbContext(new string[] { }))
            {
                var message = context.Messages.Where(m => m.DevId == devId && m.Time > from && m.Time < to)
                    .Include(m => m.Gateways).ToList();

                return message;
            }
        }
    }
}
