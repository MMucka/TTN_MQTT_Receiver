using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTCloud.Models;
using MQTTCloud.Services;

namespace MQTTCloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationsService _applicationsService;

        public ApplicationsController(ApplicationsService applicationsService)
        {
            _applicationsService = applicationsService;
        }

        // GET: api/Applications
        [HttpGet]
        public IEnumerable<Application> GetDevice()
        {
            return _applicationsService.GetAppWithDevices();
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public Application GetDevice(long id)
        {
            return _applicationsService.Find(id);
        }

        // POST: api/Applications
        [HttpPost]
        public Application AddApplication(Application device)
        {
            return _applicationsService.Add(device);
        }
        
        // POST: api/Applications/update
        [HttpPost("update")]
        public Application UpdateApplication(Application device)
        {
            return _applicationsService.Update(device);
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Application>> DeleteDevice(long id)
        {
            try
            {
                return _applicationsService.Delete(id);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}