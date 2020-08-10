using Microsoft.EntityFrameworkCore;
using MQTTCloud.Models;

namespace MQTTCloud.Models
{
    public class MessageContext:DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }

        public MessageContext() { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}