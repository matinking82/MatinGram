using EndPoint.Site.Utilities;
using MatinGram.Application.Interfaces.FacadPatterns;
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
    }
}
