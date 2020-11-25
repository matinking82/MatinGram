using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Utilities;
using MatinGram.Domain.Entities.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Messages.Commands.AddNewMessage
{
    public interface IAddNewMessageService
    {
        Task<ResultDto> ExecuteAsync(RequestAddNewMessageDto request);
    }

    public class AddNewMessageService : IAddNewMessageService
    {
        private readonly IDataBaseContext _context;
        public AddNewMessageService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto> ExecuteAsync(RequestAddNewMessageDto request)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var chatroom = await _context.Chatrooms
                    .FirstOrDefaultAsync(c => c.Guid == request.Guid);

                    var user = await _context.Users.FindAsync(request.UserId);

                    #region
                    if (user == null || chatroom == null)
                    {
                        return new ResultDto()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }

                    if (!_context.UserInChatrooms.Any(u => u.ChatroomId == chatroom.Id && u.UserId == request.UserId))
                    {
                        return new ResultDto()
                        {
                            Status = Common.Enums.ServiceStatus.AccessDenied,
                        };
                    }

                    #endregion


                    Message message = new Message()
                    {
                        Chatroom = chatroom,
                        ChatroomID = chatroom.Id,
                        SendDate = DateTime.Now,
                        Sender = user,
                        SenderId = request.UserId,
                        Text = request.Text,
                    };

                    await _context.Messages.AddAsync(message);

                    await _context.SaveChangesAsync();

                    return new ResultDto()
                    {
                        Status = Common.Enums.ServiceStatus.Success
                    };
                }
                catch (Exception)
                {
                    return new ResultDto()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }
    }

    public record RequestAddNewMessageDto
    {
        public long UserId { get; set; }
        public string Text { get; set; }
        public Guid Guid { get; set; }
    }
}
