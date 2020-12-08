using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using MatinGram.Domain.Entities.Messages;
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
        //TODO:Error Open Group
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
                        var targetUser = (await _context.UserInChatrooms
                        .Include(u => u.User)
                        .FirstOrDefaultAsync(u => u.ChatroomId == chatroom.Id && u.UserId != UserId))?.User;

                        Data.ChatroomName = targetUser.Name;


                        #region --Find Image--
                        var userImage = await _context.UserImages.FirstOrDefaultAsync(u => u.UserId == targetUser.Id);

                        Data.ImageName = (userImage != null) ?
                            userImage.ImageName :
                            "Images/UserImages/Default.png";

                        #endregion
                    }
                    else
                    {
                        Data.ChatroomName = chatroom.Name;

                        #region --Find Image--


                        var chatroomImage = await _context.ChatroomImages
                        .Where(c => c.ChatroomId == chatroom.Id)
                        .OrderBy(c => c.InsertTime)
                        .LastOrDefaultAsync();

                        Data.ImageName = chatroomImage != null ?
                            chatroomImage.ImageName :
                            "Images/ChatroomImages/Default.png";

                        #endregion

                    }

                    Data.ChatroomGuid = chatroom.Guid;
                    Data.Type = chatroom.ChatroomType;

                    #region --Take Messages--
                    List<MessageDto> MessagesData = new List<MessageDto>();

                    var messages = _context.Messages
                    .Where(m => m.ChatroomID == chatroom.Id)
                    .ToList();

                    foreach (var message in messages)
                    {

                        MessageDto messageData = new MessageDto();

                        messageData.Date = message.SendDate;
                        messageData.IsMe = message.SenderId == UserId;
                        messageData.MessageId = message.Id;
                        messageData.Text = message.Text;
                        messageData.MessageType = message.MessageType;

                        if (chatroom.ChatroomType == ChatroomType.Group)
                        {
                            var sender = _context.Users.Find(message.SenderId);

                            messageData.SenderName = sender.Name;

                            var userImage = await _context.UserImages
                            .FirstOrDefaultAsync(i => i.UserId == message.SenderId);

                            messageData.ImageName = userImage != null ?
                            userImage.ImageName :
                            "Images/UserImages/Default.png";
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
                catch (Exception e)
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

        public MessageType MessageType { get; set; }
    }
}