using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MQTTCloud.Models;
using MQTTCloud.Services;

namespace MQTTCloud.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MQTTCloud.Models.MessageContext _context;
        private readonly MessagesService _messageService;

        public IndexModel(MQTTCloud.Models.MessageContext context, MessagesService messagesService)
        {
            _context = context;
            _messageService = messagesService;
        }

        public IList<Message> Message { get;set; }

        public async Task OnGetAsync()
        {
            Message = _messageService.ListMessages().ToList();
            //return new ActionResult<IEnumerable<Message>>(_messagesService.ListMessages());
        }
    }
}
