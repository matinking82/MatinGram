using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using MatinGram.Domain.Entities.Chatrooms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername
{
    public interface IGetChatroomDetailByUsernameService
    {
        Task<ResultDto<ChatroomDetailByUsernameDto>> Execute(long MyId, string TargetUsername);
    }


    public class NewGetChatroomDetailByUsernameService : IGetChatroomDetailByUsernameService
    {
        private readonly IDataBaseContext _context;
        public NewGetChatroomDetailByUsernameService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ChatroomDetailByUsernameDto>> Execute(long MyId, string TargetUsername)
        {

            return await Task.Run(async () =>
            {
                try
                {
                    var targetUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == TargetUsername);

                    if (targetUser == null)
                    {
                        return new ResultDto<ChatroomDetailByUsernameDto>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound
                        };
                    }

                    var PVChat = await _context.Chatrooms
                    .Include(c => c.UserInChatrooms)
                    .Where(c => c.ChatroomType == ChatroomType.PV)
                    .FirstOrDefaultAsync(c => c.UserInChatrooms.Any(u => u.UserId == targetUser.Id) && c.UserInChatrooms.Any(u => u.UserId == MyId));
                    
                    
                    #region --Take Data--
                    var Data = new ChatroomDetailByUsernameDto();


                    Data.ChatroomName = targetUser.Name;

                    #region --find image--


                    var userImage = await _context.UserImages
                    .Where(u => u.UserId == targetUser.Id)
                    .OrderBy(u => u.InsertTime)
                    .LastOrDefaultAsync();

                    Data.ImageName = userImage != null ? userImage.ImageName :
                    "Images/UserImages/Default.png";


                    #endregion

                    #region --Take Messages--
                    if (PVChat != null)
                    {

                        var messages = _context.Messages
                        .Where(m => m.ChatroomID == PVChat.Id);

                        Data.Messages = messages.Select(m => new MessageDto()
                        {
                            Date = m.SendDate,
                            IsMe = m.SenderId == MyId,
                            MessageId = m.Id,
                            Text = m.Text
                        });

                        Data.ChatroomGuid = PVChat.Guid;
                    }

                    return new ResultDto<ChatroomDetailByUsernameDto>()
                    {
                        Status = ServiceStatus.Success,
                        Data = Data
                    };
                    #endregion

                    #endregion
                }
                catch (Exception)
                {
                    return new ResultDto<ChatroomDetailByUsernameDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });

        }
    }




    public class ChatroomDetailByUsernameDto
    {
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public Guid ChatroomGuid { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
    }

    public class MessageDto
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsMe { get; set; }
        public long MessageId { get; set; }
    }


    public class GetChatroomDetailByUsernameService : IGetChatroomDetailByUsernameService
    {
        private readonly IDataBaseContext _context;
        public GetChatroomDetailByUsernameService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<ChatroomDetailByUsernameDto>> Execute(long MyId, string TargetUsername)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var targetUser = await _context.Users
                    .Include(u => u.UserImages)
                    .Include(u => u.UserInChatrooms)
                    .ThenInclude(u => u.Chatroom)
                    .ThenInclude(c => c.Messages)
                    //TODO::::::::::[Find Better Way]::::::::::::::::::::
                    /**/.Include(u => u.UserInChatrooms)
                    /**/.ThenInclude(u => u.Chatroom)
                    /**/.ThenInclude(c => c.UserInChatrooms)
                    //TODO:::::::::::::::::::::::::::::::::::::::::::::::
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == TargetUsername.ToLower());

                    if (targetUser == null)
                    {
                        return new ResultDto<ChatroomDetailByUsernameDto>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound
                        };
                    }

                    var PVChat = GetPV(MyId, targetUser);

                    var Data = new ChatroomDetailByUsernameDto()
                    {
                        ChatroomName = targetUser.Name,
                        ImageName = (targetUser.UserImages != null) ? targetUser.UserImages.FirstOrDefault().ImageName
                        : "/Images/UserImages/Default.png",
                    };

                    if (PVChat != null)
                    {
                        Data.Messages = PVChat.Messages.Select(m => new MessageDto()
                        {
                            Date = m.SendDate,
                            IsMe = m.SenderId == MyId,
                            MessageId = m.Id,
                            Text = m.Text
                        });
                        Data.ChatroomGuid = PVChat.Guid;
                    }

                    return new ResultDto<ChatroomDetailByUsernameDto>()
                    {
                        Status = Common.Enums.ServiceStatus.Success,
                        Data = Data
                    };
                }
                catch (Exception)
                {
                    return new ResultDto<ChatroomDetailByUsernameDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError
                    };
                }
            });

            static Chatroom GetPV(long MyId, Domain.Entities.Users.User targetUser)
            {
                var userInChatroom = targetUser.UserInChatrooms
                    .Where(u => u.Chatroom.ChatroomType == Common.Enums.ChatroomType.PV)
                    .FirstOrDefault
                    (u => u.Chatroom.UserInChatrooms.Any(ui => ui.UserId == MyId));

                if (userInChatroom == null)
                {
                    return null;
                }

                return userInChatroom.Chatroom;
            }
        }
    }
}
