using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetUserIdByPVGuid
{
    public interface IGetUserIdByPVGuidService
    {
        Task<ResultDto<long>> ExecuteAsync(long MyID, Guid guid);
    }

    public class GetUserIdByPVGuidService : IGetUserIdByPVGuidService
    {
        private readonly IDataBaseContext _context;
        public GetUserIdByPVGuidService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<long>> ExecuteAsync(long MyId, Guid guid)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var PvChat = await _context.Chatrooms.FirstOrDefaultAsync(c => c.Guid == guid);

                    #region Validation
                    if (PvChat==null)
                    {
                        return new ResultDto<long>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound
                        };
                    }

                    if (!(PvChat.ChatroomType == Common.Enums.ChatroomType.PV))
                    {
                        return new ResultDto<long>()
                        {
                            Status = Common.Enums.ServiceStatus.Error,
                            Message = "guid وارد شده متعلق به پی وی نیست"
                        };
                    }

                    #endregion

                    var targetUserInChatroom = await _context.UserInChatrooms.FirstOrDefaultAsync(u => u.ChatroomId == PvChat.Id && u.UserId != MyId);

                    var targetUser = await _context.Users.FindAsync(targetUserInChatroom.UserId);


                    return new ResultDto<long>()
                    {
                        Status = Common.Enums.ServiceStatus.Success,
                        Data = targetUser.Id
                    };
                }
                catch (Exception)
                {
                    return new ResultDto<long>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }
    }
}
