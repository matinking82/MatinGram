using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetChatroomTypeByGuid
{
    public interface IGetChatroomTypeByGuidService
    {
        Task<ResultDto<ChatroomType>> ExecuteAsync(Guid guid);
    }


    public class GetChatroomTypeByGuidService : IGetChatroomTypeByGuidService
    {
        private readonly IDataBaseContext _context;
        public GetChatroomTypeByGuidService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ChatroomType>> ExecuteAsync(Guid guid)
        {
            return await Task.Run(async () =>
            {
                try
                {

                    var Chatroom = await _context.Chatrooms
                    .FirstOrDefaultAsync(c=> c.Guid==guid);

                    if (Chatroom==null)
                    {
                        return new ResultDto<ChatroomType>()
                        {
                            Status = ServiceStatus.NotFound,
                        };
                    }


                    return new ResultDto<ChatroomType>()
                    {
                        Status = ServiceStatus.Success,
                        Data = Chatroom.ChatroomType,
                    };


                }
                catch (Exception)
                {
                    return new ResultDto<ChatroomType>()
                    {
                        Status = ServiceStatus.SystemError,
                    };
                }
            });
        }
    }

}
