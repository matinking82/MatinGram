using EndPoint.Site.Utilities;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.Commands.CreateNewGroup;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using MatinGram.Common.Enums;
using MatinGram.ViewModels.ViewModels.Chatrooms;
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
        private readonly IUsersFacad _usersFacad;
        public ChatroomsController(IChatroomsFacad chatroomsFacad, IUsersFacad usersFacad)
        {
            _chatroomsFacad = chatroomsFacad;
            _usersFacad = usersFacad;
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

        [Route("/CreateGroup")]
        [HttpPost]
        public async Task<JsonResult> CreateGroup([Bind("GroupName,ImageFile")] CreateNewGroupViewModel group)
        {
            var UserId = User.GetUserId();

            RequestCreateNewGroupService request = new RequestCreateNewGroupService()
            {
                GroupName = group.GroupName,
                ImageFile = group.ImageFile,
                UserId = UserId,
            };

            var result = await _chatroomsFacad.CreateNewGroupService.ExecuteAsync(request);

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetProfile(Guid ChatroomGuid)
        {
            var GetTypeResult = await _chatroomsFacad.GetChatroomTypeByGuidService.ExecuteAsync(ChatroomGuid);

            if (GetTypeResult.Status == ServiceStatus.Success)
            {
                var MyUserId = User.GetUserId();

                if (GetTypeResult.Data == ChatroomType.PV)
                {
                    var GetUserIdResult = await _chatroomsFacad.GetUserIdByPVGuidService.ExecuteAsync(MyUserId, ChatroomGuid);

                    var result = await _usersFacad.GetUserPublicProfileByUserIdService.ExecuteAsync(GetUserIdResult.Data);

                    return Json(new { Data = result.Data, type = GetTypeResult.Data, Status = result.Status, Guid = ChatroomGuid });
                }
                else
                {
                    var result = await _chatroomsFacad.GetGroupDetailForProfileService.ExecuteAsync(MyUserId, ChatroomGuid);

                    return Json(new { Data = result.Data, type = GetTypeResult.Data, Status = result.Status, Guid = ChatroomGuid });
                }
            }

            return Json(GetTypeResult);
        }

        [Route("GetGroupLink/{chatroomGuid}")]
        public async Task<JsonResult> GetGroupJoinLing(Guid chatroomGuid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomJoinLinkByChatroomGuidService.ExecuteAsync(UserId, chatroomGuid);

            return Json(result);
        }

        [Route("ChangeGroupLink/{chatroomGuid}")]
        public async Task<JsonResult> ChangeGroupLink(Guid chatroomGuid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.ChangeJoinLinkGuidService.ExecuteAsync(UserId, chatroomGuid);

            return Json(result);
        }

        [Route("JoinChat/{JoinLinkGuid}")]
        [HttpGet]
        public async Task<IActionResult> JoinChat(Guid JoinLinkGuid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.JoinToChatWithLinkService.ExecuteAsync(UserId, JoinLinkGuid);

            if (result.Status != ServiceStatus.Success)
            {
                ViewBag.Message = "مشکلی پیش آمد";
                return View("ShowMessage");
            }

            return RedirectToAction("Index", "Home");
        }
        [Route("JoinChat")]
        [HttpPost]
        public async Task<JsonResult> JoinChatPost(Guid JoinLinkGuid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.JoinToChatWithLinkService.ExecuteAsync(UserId, JoinLinkGuid);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetChatroomGuidByJoinGuid(Guid JoinLinkGuid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomGuidByJoinGuidService.ExecuteAsync(UserId, JoinLinkGuid);

            return Json(result);
        }
    }
}
