using Microsoft.EntityFrameworkCore;

namespace MQTTCloud.Models
{
    public class MessageContext:DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }

        public MessageContext() { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Message> Gateways { get; set; }
    }
}