using System.Collections.Generic;

namespace MQTTCloud.Models
{
    public class Application : DbEntity
    {
        public string AppId { get; set; }
        public string AppKey{ get; set; }
        
        public ICollection<Device> Devices { get; set; } 
    }
}