using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByGuid
{
    public interface IGetChatroomDetailByGuid
    {
        Task<ResultDto<ChatroomDetailByGuidDto>> ExecuteAsync(long UserId, Guid ChatroomGuid);
    }

    public class GetChatroomDetailByGuid : IGetChatroomDetailByGuid
    {
        private readonly IDataBaseContext _context;
        public GetChatroomDetailByGuid(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ChatroomDetailByGuidDto>> ExecuteAsync(long UserId, Guid ChatroomGuid)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var chatroom = await _context.Chatrooms
                    .Include(c => c.ChatroomImages)
                    .FirstOrDefaultAsync(c => c.Guid == ChatroomGuid);

                    #region --Validation--
                    if (chatroom == null || !_context.Users.Any(u => u.Id == UserId))
                    {
                        return new ResultDto<ChatroomDetailByGuidDto>()
                        {
                            Status = ServiceStatus.NotFound,
                        };
                    }

                    //if (!chatroom.UserInChatrooms.Any(u => u.UserId == UserId))
                    if (!_context.UserInChatrooms.Any(u => u.UserId == UserId && u.ChatroomId == chatroom.Id))
                    {
                        return new ResultDto<ChatroomDetailByGuidDto>()
                        {
                            Status = ServiceStatus.AccessDenied,
                        };
                    }
                    #endregion

                    #region --Fill Data--
                    ChatroomDetailByGuidDto Data = new ChatroomDetailByGuidDto();

                    if (chatroom.ChatroomType == ChatroomType.PV)
                    {
                        var targetUser = await _context.Users
                        .Include(u => u.UserInChatrooms)
                        .Include(u => u.UserImages)
                        .FirstOrDefaultAsync(u => u.UserInChatrooms.Any(c => c.UserId != UserId && c.ChatroomId == chatroom.Id));

                        Data.ChatroomName = targetUser.Name;

                        Data.ImageName = (targetUser.UserImages != null) ?
                            targetUser.UserImages.FirstOrDefault().ImageName :
                            "/Images/UserImages/Default.png";

                    }
                    else
                    {
                        Data.ChatroomName = chatroom.Name;

                        Data.ImageName = (chatroom.ChatroomImages != null) ?
                            chatroom.ChatroomImages.FirstOrDefault().ImageName :
                            "/Images/ChatroomImages/Default.png";
                    }

                    Data.ChatroomGuid = chatroom.Guid;
                    Data.Type = chatroom.ChatroomType;

                    #region --Take Messages--
                    List<MessageDto> MessagesData = new List<MessageDto>();
                    var messages = await _context.Messages
                    .Include(m => m.Sender)
                    .Where(m => m.ChatroomID == chatroom.Id)
                    .ToListAsync();
                    foreach (var message in messages)
                    {

                        MessageDto messageData = new MessageDto();

                        messageData.Date = message.SendDate;
                        messageData.IsMe = message.SenderId == UserId;
                        messageData.MessageId = message.Id;
                        messageData.Text = message.Text;

                        if (chatroom.ChatroomType == ChatroomType.Group)
                        {
                            messageData.SenderName = message.Sender.Name;
                            
                            var userImage = await _context.UserImages
                            .FirstOrDefaultAsync(i => i.UserId == message.SenderId);

                            messageData.ImageName = userImage!=null?
                            userImage.ImageName:
                            "/Images/UserImages/Default.png";

                        }


                        MessagesData.Add(messageData);
                    }
                    #endregion


                    Data.Messages = MessagesData;


                    #endregion



                    return new ResultDto<ChatroomDetailByGuidDto>()
                    {
                        Data = Data,
                        Status = ServiceStatus.Success,
                    };

                }
                catch (Exception)
                {
                    return new ResultDto<ChatroomDetailByGuidDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }

            });
        }
    }



    public class ChatroomDetailByGuidDto
    {
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public Guid ChatroomGuid { get; set; }
        public ChatroomType Type { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
    }

    public class MessageDto
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsMe { get; set; }
        public long MessageId { get; set; }

        public string ImageName { get; set; }
        public string SenderName { get; set; }
    }
}
