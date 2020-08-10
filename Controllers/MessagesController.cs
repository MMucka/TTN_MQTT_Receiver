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

        public MessagesController(MessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return new ActionResult<IEnumerable<Message>>(_messagesService.ListMessages());
        }

        //GET: api/Messages/startDate/endDate
        [HttpGet("{startDateString}/{endDateString}")]
        public IEnumerable<Message> GetByDateRange(string startDateString, string endDateString)
        {
            Console.WriteLine(startDateString + " - " + endDateString);
            var startDate = BuildDateTimeFromYAFormat(startDateString);
            var endDate = BuildDateTimeFromYAFormat(endDateString);

            Console.WriteLine(startDate + " - " + endDate);


            return _messagesService.ListDatesMessages(startDate, endDate);
        }

        private DateTime BuildDateTimeFromYAFormat(string dateString)
        {
            Regex r = new Regex(@"^\d{4}\d{2}\d{2}T\d{2}\d{2}Z$");
            if (!r.IsMatch(dateString))
            {
                throw new FormatException($"{dateString} is not the correct format. Should be yyyyMMddThhmmZ");
            }

            return DateTime.ParseExact(dateString, "yyyyMMddThhmmZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        }

        [HttpGet("device/{devId}")]
        public IEnumerable<Message> GetByDeviceID(string devId)
        {
            return _messagesService.ListDeviceMessages(devId);
        }


        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteMessage(long id)
        {
            var message = _messagesService.DeleteMessage(id);
            if (message == null)
            {
                return NotFound();
            }

            return message;
        }
    }
}
