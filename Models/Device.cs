#nullable enable
using System;

namespace MQTTCloud.Models
{
    public class Device : DbEntity
    {
        public String DevID { get; set; }
        
        public bool Active { get; set; }

        public long ApplicationId { get; set; }

        public byte[]? AesKey { get; set; }
        
        public byte[]? AesIv { get; set; }
    }
}