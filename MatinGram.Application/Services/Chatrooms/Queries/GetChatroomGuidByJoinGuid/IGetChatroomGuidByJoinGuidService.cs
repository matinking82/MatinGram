using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetChatroomGuidByJoinGuid
{
    public interface IGetChatroomGuidByJoinGuidService
    {
        Task<ResultDto<Guid>> ExecuteAsync(long MyUserId, Guid ChatroomJoinGuid);
    }

    public class GetChatroomGuidByJoinGuidService : IGetChatroomGuidByJoinGuidService
    {
        private readonly IDataBaseContext _context;
        public GetChatroomGuidByJoinGuidService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<Guid>> ExecuteAsync(long MyUserId, Guid ChatroomJoinGuid)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var chatroom = await _context.Chatrooms
                    .FirstOrDefaultAsync(c => c.JoinLinkGuid == ChatroomJoinGuid);


                    #region Validation
                    if (chatroom == null || chatroom.ChatroomType == Common.Enums.ChatroomType.PV)
                    {
                        return new ResultDto<Guid>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }
                    if (!_context.UserInChatrooms.Any(u => u.ChatroomId == chatroom.Id && u.UserId == MyUserId))
                    {
                        return new ResultDto<Guid>()
                        {
                            Status = Common.Enums.ServiceStatus.AccessDenied,
                        };
                    }
                    #endregion

                    return new ResultDto<Guid>()
                    {
                        Status = Common.Enums.ServiceStatus.Success,
                        Data = chatroom.Guid,
                    };
                }
                catch (Exception)
                {
                    return new ResultDto<Guid>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }
    }
}
