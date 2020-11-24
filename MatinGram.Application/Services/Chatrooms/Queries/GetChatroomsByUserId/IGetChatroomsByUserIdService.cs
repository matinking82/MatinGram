using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetChatroomsByUserId
{
    public interface IGetChatroomsByUserIdService
    {
        Task<ResultDto<IEnumerable<ResultGetChatroomsByUserId>>> Execute(long UserId);
    }

    public class GetChatroomsByUserIdService : IGetChatroomsByUserIdService
    {
        private readonly IDataBaseContext _context;
        public GetChatroomsByUserIdService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<IEnumerable<ResultGetChatroomsByUserId>>> Execute(long UserId)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var user = await _context.Users
                    .Include(c => c.UserInChatrooms)
                    .ThenInclude(u => u.Chatroom)
                    .ThenInclude(c => c.Creator)
                    .FirstOrDefaultAsync(u => u.Id == UserId);

                    if (user == null)
                    {
                        return new ResultDto<IEnumerable<ResultGetChatroomsByUserId>>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound
                        };
                    }

                    var chatrooms = user.UserInChatrooms
                    .Select(u => new ResultGetChatroomsByUserId()
                    {

                        ChatroomName = (u.Chatroom.ChatroomType == ChatroomType.PV) ?
                        GetUser(UserId, u).Name
                        : u.Chatroom.Name,

                        //check is pv 
                        ImageName = (u.Chatroom.ChatroomType == ChatroomType.PV) ?
                        //pv
                        ((GetUser(UserId, u).UserImages !=null) ?GetUser(UserId, u).UserImages.FirstOrDefault().ImageName
                        : "/images/UserImages/Default.png")
                        //not pv
                        :((u.Chatroom.ChatroomImages!=null)? u.Chatroom.ChatroomImages.FirstOrDefault().ImageName 
                         :"/images/ChatroomImages/Default.png") ,

                        LastMessage = u.Chatroom.Messages.LastOrDefault().Text,
                        LastMessageTime = u.Chatroom.Messages.LastOrDefault().SendDate,
                        Guid = u.Chatroom.Guid
                    });


                    return new ResultDto<IEnumerable<ResultGetChatroomsByUserId>>()
                    {
                        Data = chatrooms,
                        Status = ServiceStatus.Success
                    };


                }
                catch (Exception)
                {
                    return new ResultDto<IEnumerable<ResultGetChatroomsByUserId>>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError
                    };
                }
            });

            static Domain.Entities.Users.User GetUser(long UserId, Domain.Entities.Relations.UserInChatroom u)
            {
                return u.Chatroom.UserInChatrooms.FirstOrDefault(u => u.UserId != UserId).User;
            }
        }
    }

    public record ResultGetChatroomsByUserId
    {
        public string LastMessage { get; set; }
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public DateTime LastMessageTime { get; set; }
        public Guid Guid { get; set; }
    }
}
