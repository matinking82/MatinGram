using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Commands.CreateNewChatroomPV
{
    public interface ICreateNewChatroomPVService
    {
        Task<ResultDto<Guid>> ExecuteAsync(long MyUserId, string TargetUsername);
    }

    public class CreateNewChatroomPVService : ICreateNewChatroomPVService
    {
        private readonly IDataBaseContext _context;
        public CreateNewChatroomPVService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<Guid>> ExecuteAsync(long MyUserId, string TargetUsername)
        {
            return await Task.Run(async () =>
            {
                try
                {

                    var targetUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Username == TargetUsername);

                    var myUser = await _context.Users.FindAsync(MyUserId);

                    if (targetUser == null || myUser == null)
                    {
                        return new ResultDto<Guid>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }

                    Chatroom oldChatroom = await _context.Chatrooms
                        .Include(c => c.UserInChatrooms)
                        .Where(c => c.ChatroomType == Common.Enums.ChatroomType.PV)
                        .FirstOrDefaultAsync(c => c.UserInChatrooms.Any(u => u.UserId == targetUser.Id) && c.UserInChatrooms.Any(u => u.UserId == MyUserId));

                    Guid Data = oldChatroom.Guid;

                    if (oldChatroom == null)
                    {

                        Chatroom newChatroom = new Chatroom()
                        {
                            ChatroomType = Common.Enums.ChatroomType.PV,
                            Guid = Guid.NewGuid(),
                            InsertTime = DateTime.Now,

                        };
                        await _context.Chatrooms.AddAsync(newChatroom);

                        UserInChatroom myUserInChatroom = new UserInChatroom()
                        {
                            Chatroom = newChatroom,
                            ChatroomId = newChatroom.Id,
                            User = myUser,
                            UserId = myUser.Id
                        };

                        UserInChatroom targetUserInChatroom = new UserInChatroom()
                        {
                            Chatroom = newChatroom,
                            ChatroomId = newChatroom.Id,
                            User = targetUser,
                            UserId = targetUser.Id
                        };

                        await _context.UserInChatrooms.AddAsync(myUserInChatroom);
                        await _context.UserInChatrooms.AddAsync(targetUserInChatroom);


                        await _context.SaveChangesAsync();

                        Data = newChatroom.Guid;
                    }


                    return new ResultDto<Guid>()
                    {
                        Data = Data,
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
