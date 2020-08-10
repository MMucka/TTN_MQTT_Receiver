using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQTTCloud.Models;
using MQTTCloud.MQTT;
using MQTTCloud.Services;
using System.Security.Cryptography;

namespace MQTTCloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesService _devicesService;
        private readonly BlockingDevices _blockingDevices;

        public DevicesController(DevicesService devicesService, BlockingDevices blockingDevices)
        {
            _devicesService = devicesService;
            _blockingDevices = blockingDevices;
        }

        // GET: api/Devices        -- get all
        [HttpGet]
        public IEnumerable<Device> GetDevice()
        {
            return _devicesService.List();
        }

        // GET: api/Devices/5      -- get one
        [HttpGet("{id}")]
        public Device GetDevice(long id)
        {
            return _devicesService.Find(id);
        }

        // POST: api/Devices        -- new device
        [HttpPost]
        public Device AddDevice(Device device)
        {
            return _devicesService.Add(device);
        }
        
        // GET: api/Devices/id/activate
        [HttpGet("{id}/activate")]
        public Device ActivateDevice(long id)
        {
            return _devicesService.ActivateDevice(id);
        }
        
        // GET: api/Devices/id/deactivate
        [HttpGet("{id}/deactivate")]
        public Device DeactivateDevice(long id)
        {
            return _devicesService.DeactivateDevice(id);
        }
        
        // DELETE: api/Devices/5
        [HttpGet("{id}/delete")]
        public async Task<ActionResult<Device>> DeleteDevice(long id)
        {
            try
            {
                _devicesService.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
