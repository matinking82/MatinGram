using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatinGram.ViewModels.ViewModels.Messages;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Messages.Commands.AddNewMessage;
using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace EndPoint.Site.Controllers
{
    [Authorize("User")]
    public class MessagesController : Controller
    {
        private readonly IMessagesFacad _messagesFacad;
        public MessagesController(IMessagesFacad messagesFacad)
        {
            _messagesFacad = messagesFacad;
        }

        [HttpPost]
        public async Task<JsonResult> AddMessage([Bind("Text,Guid")] AddNewMessageViewModel Message)
        {
            RequestAddNewMessageDto request = new RequestAddNewMessageDto()
            {
                Guid = Message.Guid,
                Text = Message.Text,
                UserId = User.GetUserId()
            };

            var result = await _messagesFacad.AddNewMessageService.ExecuteAsync(request);

            return Json(new { Status = result.Status });
        }
    }
}
