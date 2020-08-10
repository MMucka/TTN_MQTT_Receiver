using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MQTTCloud.Models
{
    public class Gateway : DbEntity
    {
        public string GtwId { get; set; }
        public long Timestamp { get; set; }
        public int Rssi { get; set; }
        public float Snr { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public long MessageId { get; set; }
    }
}
