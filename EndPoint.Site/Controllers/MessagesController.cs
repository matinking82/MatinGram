using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatinGram.ViewModels.ViewModels.Messages;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Messages.Commands.AddNewMessage;
using EndPoint.Site.Utilities;

namespace EndPoint.Site.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesFacad _messagesFacad;
        public MessagesController(IMessagesFacad messagesFacad)
        {
            _messagesFacad = messagesFacad;
        }

        [HttpPost]
        public async Task<JsonResult> AddMessage([Bind("Text,Guid")] AddNewMessageViewModel message)
        {
            RequestAddNewMessageDto request = new RequestAddNewMessageDto()
            {
                Guid = message.Guid,
                Text = message.Text,
                UserId = User.GetUserId()
            };

            var result = await _messagesFacad.AddNewMessageService.ExecuteAsync(request);

            return Json(result);
        }
    }
}
