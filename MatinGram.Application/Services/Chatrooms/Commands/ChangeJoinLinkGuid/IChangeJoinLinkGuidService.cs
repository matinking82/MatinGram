using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Commands.ChangeJoinLinkGuid
{
    public interface IChangeJoinLinkGuidService
    {
        Task<ResultDto<Guid>> ExecuteAsync(long MyUserId, Guid ChatroomGuid);
    }

    public class ChangeJoinLinkGuidService : IChangeJoinLinkGuidService
    {
        private readonly IDataBaseContext _context;
        public ChangeJoinLinkGuidService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<Guid>> ExecuteAsync(long MyUserId, Guid ChatroomGuid)
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
                        return new ResultDto<Guid>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }

                    if (_context.AdminInChatrooms.Any(a => a.UserId == MyUserId && a.ChatroomId == chatroom.Id))
                    {
                        return new ResultDto<Guid>()
                        {
                            Status = Common.Enums.ServiceStatus.AccessDenied,
                        };
                    }
                    #endregion


                    chatroom.JoinLinkGuid = Guid.NewGuid();

                    await _context.SaveChangesAsync();

                    return new ResultDto<Guid>()
                    {
                        Data = chatroom.JoinLinkGuid,
                        Status = Common.Enums.ServiceStatus.Success,
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
