using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetChatroomJoinLinkByChatroomGuid
{
    public interface IGetChatroomJoinLinkByChatroomGuidService
    {
        Task<ResultDto<ChatroomJoinLinkDto>> ExecuteAsync(long MyUserId, Guid ChatroomGuid);
    }

    public class GetChatroomJoinLinkByChatroomGuidService : IGetChatroomJoinLinkByChatroomGuidService
    {
        private readonly IDataBaseContext _context;
        public GetChatroomJoinLinkByChatroomGuidService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ChatroomJoinLinkDto>> ExecuteAsync(long MyUserId, Guid ChatroomGuid)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var chatroom = await _context.Chatrooms
                    .FirstOrDefaultAsync(c => c.Guid == ChatroomGuid);

                    #region validation 
                    if (chatroom == null || chatroom.ChatroomType == Common.Enums.ChatroomType.PV)
                    {
                        return new ResultDto<ChatroomJoinLinkDto>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }

                    if (!(chatroom.CreatorId == MyUserId || _context.AdminInChatrooms.Any(a => a.ChatroomId == chatroom.Id && a.UserId == MyUserId)))
                    {
                        return new ResultDto<ChatroomJoinLinkDto>()
                        {
                            Status = Common.Enums.ServiceStatus.AccessDenied,                            
                        };
                    }

                    #endregion


                    return new ResultDto<ChatroomJoinLinkDto>()
                    {
                        Status = Common.Enums.ServiceStatus.Success,
                        Data = new ChatroomJoinLinkDto()
                        {
                            ChatroomJoinLinkGuid = chatroom.JoinLinkGuid
                        },
                    };

                }
                catch (Exception)
                {
                    return new ResultDto<ChatroomJoinLinkDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }
    }

    public record ChatroomJoinLinkDto
    {
        public Guid ChatroomJoinLinkGuid { get; set; }
    }
}
