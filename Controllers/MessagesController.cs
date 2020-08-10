using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQTTCloud.Models;
using MQTTCloud.Services;

namespace MQTTCloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesService _messagesService;
        private readonly GatewaysService _gatewaysService;

        public MessagesController(MessagesService messagesService, GatewaysService gatewaysService)
        {
            _messagesService = messagesService;
            _gatewaysService = gatewaysService;
        }

        // GET: api/Messages
        [HttpGet]
        public string GetMessages()
        {
            return _messagesService.List();
        }
        
        private DateTime BuildDateTimeFromYAFormat(string dateString)
        {
            Regex r = new Regex(@"^\d{4}\d{2}\d{2}T\d{2}\d{2}$");
            if (!r.IsMatch(dateString))
            {
                throw new FormatException($"{dateString} is not the correct format. Should be yyyyMMddTHHmm");
            }

            return DateTime.ParseExact(dateString, "yyyyMMddTHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None);

        }


        //GET: api/Messages/id/startDate/endDate        http://localhost:8080/api/messages/2/20191120T1105/20191220T1110
        [HttpGet("{id}/{startDateString}/{endDateString}")]
        public string GetByDateRange(long id, string startDateString, string endDateString)
        {
            try
            {
                var startDate = BuildDateTimeFromYAFormat(startDateString);
                var endDate = BuildDateTimeFromYAFormat(endDateString);

                Console.WriteLine(startDate + " - " + endDate);
                return _messagesService.ListDeviceDatesMessages(id, startDate, endDate);
            }
            catch (System.FormatException e)
            {
                Console.WriteLine(e.ToString());
            }

            return "Error";
        }

        //GET: api/messages/id
        [HttpGet("{devId}")]
        public string GetByDeviceId(long devId)
        {
            return _messagesService.ListDeviceMessages(devId);
        }
        
        //GET: api/messages/gateways/messageId
        [HttpGet("gateways/{msgId}")]
        public IEnumerable<Gateway> GetGatewaysByMessageId(long msgId)
        {
            return _gatewaysService.GetMessageGateways(msgId);
        }


        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteMessage(long id)
        {
            try
            {
                return _messagesService.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Message not found " + e.ToString());
                return NotFound();
            }
        }
    }
}
