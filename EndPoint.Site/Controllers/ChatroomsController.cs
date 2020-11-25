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

            return Json(result);
        }


        [Route("/OpenChat/{Guid}")]
        public async Task<JsonResult> OpenChat(Guid Guid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomDetailByGuid.ExecuteAsync(UserId, Guid);

            return Json(result);
        }


        [Route("/CreatePV/{Username}")]
        [HttpPost]
        public async Task<JsonResult> CreatePV(string Username)
        {
            long UserId = User.GetUserId();

            var result = await _chatroomsFacad.CreateNewChatroomPVService.ExecuteAsync(UserId, Username);

            return Json(result);
        }



    }
}
