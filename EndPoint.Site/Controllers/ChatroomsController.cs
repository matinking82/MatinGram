using EndPoint.Site.Utilities;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class ChatroomsController : Controller
    {
        private readonly IChatroomsFacad _chatroomsFacad;
        public ChatroomsController(IChatroomsFacad chatroomsFacad)
        {
            _chatroomsFacad = chatroomsFacad;
        }

        public async Task<JsonResult> GetChatroomsList()
        {

            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomsByUserIdService.Execute(UserId);

            return Json(result);
        }
        [Route("/OpenPV/{Username}")]
        public async Task<JsonResult> OpenChat(string Username)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomDetailByUsernameService.Execute(UserId, Username);

            result.Data.Messages = new List<MessageDto>()
            {
                new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },
new MessageDto()
                {
                    Text = "test",
                    IsMe = true,
                    Date = DateTime.Now,
                },
                new MessageDto()
                {
                    Text = "test",
                    Date = DateTime.Now,
                    IsMe = false
                },

            };

            return Json(result);
        }

        [Route("/OpenChat/{Guid}")]
        public async Task<JsonResult> OpenChat(Guid Guid)
        {
            var UserId = User.GetUserId();

            return Json(new { });
        }
    }
}
