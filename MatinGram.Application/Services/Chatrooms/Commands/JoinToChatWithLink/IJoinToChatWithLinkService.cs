using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Commands.JoinToChatWithLink
{
    public interface IJoinToChatWithLinkService
    {
        Task<ResultDto<JoinedChatDetailDto>> ExecuteAsync(long MyUserId, Guid ChatroomJoinGuid);
    }

    public class JoinToChatWithLinkService : IJoinToChatWithLinkService
    {
        private readonly IDataBaseContext _context;
        public JoinToChatWithLinkService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<JoinedChatDetailDto>> ExecuteAsync(long MyUserId, Guid ChatroomJoinGuid)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var chatroom = await _context.Chatrooms
                    .FirstOrDefaultAsync(c => c.JoinLinkGuid == ChatroomJoinGuid);

                    var user = await _context.Users.FindAsync(MyUserId);

                    #region validation
                    if (chatroom == null || chatroom.ChatroomType == Common.Enums.ChatroomType.PV || user == null)
                    {
                        return new ResultDto<JoinedChatDetailDto>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }

                    if (_context.UserInChatrooms.Any(u => u.UserId == MyUserId && u.ChatroomId == chatroom.Id))
                    {
                        return new ResultDto<JoinedChatDetailDto>()
                        {
                            Status = Common.Enums.ServiceStatus.AccessDenied,
                        };
                    }
                    #endregion


                    UserInChatroom newUserInChatroom = new UserInChatroom()
                    {
                        Chatroom = chatroom,
                        ChatroomId = chatroom.Id,
                        User = user,
                        UserId = MyUserId
                    };

                    await _context.UserInChatrooms.AddAsync(newUserInChatroom);


                    await _context.SaveChangesAsync();


                    JoinedChatDetailDto Data = new JoinedChatDetailDto()
                    {
                        ChatroomName = chatroom.Name,
                        Guid = chatroom.Guid,
                    };

                    #region find image 

                    var chatroomImage = await _context.ChatroomImages
                    .FirstOrDefaultAsync(c => c.ChatroomId == chatroom.Id);

                    string chatroomImageName = "Images/ChatroomImages/Defaut.png";
                    if (chatroomImage != null)
                    {
                        chatroomImageName = chatroomImage.ImageName;
                    }
                    #endregion

                    Data.ImageName = chatroomImageName;

                    #region find last message 

                    var lastmessage = await _context.Messages
                    .Where(m => m.ChatroomID == chatroom.Id)
                    .OrderBy(m => m.SendDate)
                    .LastOrDefaultAsync();



                    #endregion
                    Data.LastMessage = lastmessage?.Text;
                    Data.LastMessageTime = lastmessage?.SendDate;

                    return new ResultDto<JoinedChatDetailDto>()
                    {
                        Status = Common.Enums.ServiceStatus.Success,
                        Data = Data,
                    };

                }
                catch (Exception)
                {
                    return new ResultDto<JoinedChatDetailDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }
    }

    public record JoinedChatDetailDto
    {
        public string LastMessage { get; set; }
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public Guid Guid { get; set; }
    }
}
