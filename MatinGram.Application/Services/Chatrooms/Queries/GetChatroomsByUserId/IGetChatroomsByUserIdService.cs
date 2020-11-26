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

    public class NewGetChatroomsByUserIdService : IGetChatroomsByUserIdService
    {
        private readonly IDataBaseContext _context;
        public NewGetChatroomsByUserIdService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<IEnumerable<ResultGetChatroomsByUserId>>> Execute(long UserId)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var userInChatrooms = _context.UserInChatrooms
                    .Where(u => u.UserId == UserId).ToList();

                    if (userInChatrooms.Count() < 1)
                    {
                        return new ResultDto<IEnumerable<ResultGetChatroomsByUserId>>()
                        {
                            Status = ServiceStatus.NotFound,
                        };
                    }

                    List<ResultGetChatroomsByUserId> Data = new List<ResultGetChatroomsByUserId>();

                    #region --Fill Data--
                    foreach (var userInChatroom in userInChatrooms)
                    {
                        ResultGetChatroomsByUserId res = new ResultGetChatroomsByUserId();

                        var chatroom = await _context.Chatrooms
                        .FindAsync(userInChatroom.ChatroomId);

                        if (chatroom.ChatroomType == ChatroomType.PV)
                        {
                            #region --Find TargetUser--
                            var targetUser = (await _context.UserInChatrooms
                            .Include(u => u.User)
                            .FirstOrDefaultAsync(u => u.ChatroomId == chatroom.Id && u.UserId != UserId)).User;
                            #endregion

                            res.ChatroomName = targetUser.Name;

                            #region --Find Image--
                            var userImage = await _context.UserImages
                            .Where(u => u.UserId == targetUser.Id)
                            .OrderBy(u=> u.InsertTime)
                            .LastOrDefaultAsync();

                            if (userImage != null)
                            {
                                res.ImageName = userImage.ImageName;
                            }
                            else
                            {
                                res.ImageName = "/Images/UserImages/Default.png";
                            }
                            #endregion
                        }
                        else
                        {
                            res.ChatroomName = chatroom.Name;

                            #region --Find Image--
                            var chatroomImage = await _context.ChatroomImages.LastOrDefaultAsync(c => c.ChatroomId == chatroom.Id);
                            if (chatroomImage != null)
                            {
                                res.ImageName = chatroomImage.ImageName;
                            }
                            else
                            {
                                res.ImageName = "/Images/ChatroomImages/Default.png";
                            }

                            #endregion
                        }
                        #region --Find Last Message--
                        var LastMessage = await _context.Messages
                        .Where(m => m.ChatroomID == chatroom.Id)
                        .OrderBy(m=> m.SendDate)
                        .LastOrDefaultAsync();

                        if (LastMessage != null)
                        {
                            res.LastMessage = LastMessage.Text;
                            res.LastMessageTime = LastMessage.SendDate;
                        }
                        #endregion

                        res.Guid = chatroom.Guid;


                        Data.Add(res);
                    }

                    #endregion

                    return new ResultDto<IEnumerable<ResultGetChatroomsByUserId>>()
                    {
                        Status = ServiceStatus.Success,
                        Data = Data
                    };
                }
                catch (Exception)
                {
                    return new ResultDto<IEnumerable<ResultGetChatroomsByUserId>>()
                    {
                        Status = ServiceStatus.SystemError,
                    };
                }
            });
        }
    }


    public record ResultGetChatroomsByUserId
    {
        public string LastMessage { get; set; }
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public Guid Guid { get; set; }
    }


    public class OldGetChatroomsByUserIdService : IGetChatroomsByUserIdService
    {
        private readonly IDataBaseContext _context;
        public OldGetChatroomsByUserIdService(IDataBaseContext context)
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
                    //TODO::::::::::[Find Better Way]::::::::::::::::::::
                    /**/.Include(c => c.UserInChatrooms)
                    /**/.ThenInclude(u => u.Chatroom)
                    /**/.ThenInclude(c => c.ChatroomImages)
                    //TODO::::::::::[Find Better Way]::::::::::::::::::::

                    .Include(c => c.UserInChatrooms)
                    .ThenInclude(u => u.Chatroom)
                    .ThenInclude(c => c.UserInChatrooms)
                    .ThenInclude(u => u.User)
                    .ThenInclude(u => u.UserImages)
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
                        ((GetUser(UserId, u).UserImages != null) ? GetUser(UserId, u).UserImages.FirstOrDefault().ImageName
                        : "/images/UserImages/Default.png")
                        //not pv
                        : ((u.Chatroom.ChatroomImages != null) ? u.Chatroom.ChatroomImages.FirstOrDefault().ImageName
                         : "/images/ChatroomImages/Default.png"),

                        LastMessage = u.Chatroom.Messages?.LastOrDefault()?.Text,
                        LastMessageTime = u.Chatroom.Messages?.LastOrDefault()?.SendDate,
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
}
