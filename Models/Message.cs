using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MQTTCloud.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string DevId { get; set; }
        public long Airtime { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime Time { get; set; }
        public ICollection<Gateway> Gateways { get; set; }
    }
}
