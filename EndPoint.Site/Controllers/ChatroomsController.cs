using EndPoint.Site.Utilities;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.Commands.CreateNewGroup;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using MatinGram.Common.Enums;
using MatinGram.ViewModels.ViewModels.Chatrooms;
using MatinGram.ViewModels.ViewModels.Messages;
using MatinGram.ViewModels.ViewModels.Users;
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

            IEnumerable<ChatroomsListViewModel> Data = result.Data.Select(c => new ChatroomsListViewModel()
            {
                ChatroomName = c.ChatroomName,
                Guid = c.Guid,
                ImageName = c.ImageName,
                LastMessage = c.LastMessage,
                LastMessageTime = c.LastMessageTime
            });

            return Json(new { Status = result.Status, Data = Data });
        }

        [Route("/OpenPV/{Username}")]
        public async Task<JsonResult> OpenChat(string Username)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomDetailByUsernameService.Execute(UserId, Username);


            ChatroomPVDetailsViewModel Data = new ChatroomPVDetailsViewModel()
            {
                ChatroomGuid = result.Data.ChatroomGuid,
                ChatroomName = result.Data.ChatroomName,
                ImageName = result.Data.ImageName,
                Messages = result.Data.Messages?.Select(m => new PVMessageViewModel()
                {
                    Date = m.Date,
                    IsMe = m.IsMe,
                    MessageId = m.MessageId,
                    Text = m.Text,
                    MessageType = m.MessageType
                }).ToList(),
            };

            return Json(new { Status = result.Status, Data = Data });
        }


        [Route("/OpenChat/{Guid}")]
        public async Task<JsonResult> OpenChat(Guid Guid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomDetailByGuid.ExecuteAsync(UserId, Guid);

            ChatroomDetailsViewModel Data = new ChatroomDetailsViewModel()
            {
                ChatroomGuid = result.Data.ChatroomGuid,
                ChatroomName = result.Data.ChatroomName,
                ImageName = result.Data.ImageName,
                Type = result.Data.Type,
                Messages = result.Data.Messages.Select(m => new ChatroomMessageViewModel()
                {
                    Date = m.Date,
                    ImageName = m.ImageName,
                    IsMe = m.IsMe,
                    MessageId = m.MessageId,
                    SenderName = m.SenderName,
                    Text = m.Text,
                    MessageType = m.MessageType
                }),
            };

            return Json(new { Status = result.Status, Data = Data });
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

            return Json(new { Status = result.Status });
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
                    #region Return User Profile
                    var GetUserIdResult = await _chatroomsFacad.GetUserIdByPVGuidService.ExecuteAsync(MyUserId, ChatroomGuid);
                    var result = await _usersFacad.GetUserPublicProfileByUserIdService.ExecuteAsync(GetUserIdResult.Data);

                    UserPublicProfileViewModel Data = new UserPublicProfileViewModel()
                    {
                        Bio = result.Data.Bio,
                        ImageName = result.Data.ImageName,
                        Name = result.Data.Name,
                        UserHaskKey = result.Data.UserHaskKey,
                        Username = result.Data.Username,
                    };

                    return Json(new { Data = Data, type = GetTypeResult.Data, Status = result.Status, Guid = ChatroomGuid });
                    #endregion
                }
                else
                {
                    #region Return Group Profile
                    var result = await _chatroomsFacad.GetGroupDetailForProfileService.ExecuteAsync(MyUserId, ChatroomGuid);

                    GroupDetailForProfileViewModel Data = new GroupDetailForProfileViewModel()
                    {
                        GroupName = result.Data.GroupName,
                        ImageName = result.Data.ImageName,
                        MyLevel = result.Data.MyLevel,
                        Members = result.Data.Members.Select(m => new GroupMemberViewModel()
                        {
                            HashKey = m.HashKey,
                            MemberLevel = m.MemberLevel,
                            ImageName = m.ImageName,
                            Name = m.Name,
                        }),
                    };

                    return Json(new { Data = Data, type = GetTypeResult.Data, Status = result.Status, Guid = ChatroomGuid });
                    #endregion
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

            return Json(new { Status = result.Status, Data = result.Data });
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
            if (result.Data != null)
            {
                JoinedChatDetailsViewModel Data = new JoinedChatDetailsViewModel()
                {
                    ChatroomName = result.Data.ChatroomName,
                    Guid = result.Data.Guid,
                    ImageName = result.Data.ImageName,
                    LastMessage = result.Data.LastMessage,
                    LastMessageTime = result.Data.LastMessageTime,
                };
                return Json(new { Status = result.Status, Data = Data });
            }
            else
            {
                return Json(new { Status = result.Status });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetChatroomGuidByJoinGuid(Guid JoinLinkGuid)
        {
            var UserId = User.GetUserId();

            var result = await _chatroomsFacad.GetChatroomGuidByJoinGuidService.ExecuteAsync(UserId, JoinLinkGuid);

            return Json(new { Status = result.Status, Data = result.Data });
        }
    }
}
