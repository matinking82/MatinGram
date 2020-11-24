using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
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
                    .Include(u => u.UserInChatrooms)
                    .ThenInclude(u => u.Chatroom)
                    .ThenInclude(c => c.Messages)
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
                        ImageName = (targetUser.UserImages != null) ? targetUser.UserImages.FirstOrDefault().ImageName : "/Images/UserImages/Default.png",
                        Messages = new List<MessageDto>()
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

    public class ChatroomDetailByUsernameDto
    {
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
    }

    public class MessageDto
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsMe { get; set; }
        public long MessageId { get; set; }
    }
}
