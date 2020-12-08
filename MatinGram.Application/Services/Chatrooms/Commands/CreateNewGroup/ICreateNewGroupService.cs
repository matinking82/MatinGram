using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Utilities;
using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Messages;
using MatinGram.Domain.Entities.Relations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Commands.CreateNewGroup
{
    public interface ICreateNewGroupService
    {
        Task<ResultDto> ExecuteAsync(RequestCreateNewGroupService request);
    }

    public class CreateNewGroupService : ICreateNewGroupService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public CreateNewGroupService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<ResultDto> ExecuteAsync(RequestCreateNewGroupService request)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    #region Find User
                    var Creator = await _context.Users.FindAsync(request.UserId);
                    #endregion

                    #region Validate
                    if (Creator == null)
                    {
                        return new ResultDto()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound
                        };
                    }

                    if (String.IsNullOrWhiteSpace(request.GroupName))
                    {
                        return new ResultDto()
                        {
                            Status = Common.Enums.ServiceStatus.Error,
                            Message = "نام وارد شده اشتباه اسنت"
                        };
                    }

                    #endregion

                    #region --Create Chatroom--
                    Chatroom newChatroom = new Chatroom
                    {
                        Name = request.GroupName,
                        ChatroomType = Common.Enums.ChatroomType.Group,
                        CreatorId = request.UserId,
                        Creator = Creator,
                        Guid = Guid.NewGuid(),
                        InsertTime = DateTime.Now,
                    };
                    await _context.Chatrooms.AddAsync(newChatroom);

                    Message info = new Message()
                    {
                        Chatroom = newChatroom,
                        ChatroomID = newChatroom.Id,
                        MessageType = MessageType.Info,
                        SendDate = DateTime.Now,
                        Text = $"گروه {newChatroom.Name} ایجاد شد"
                    };

                    await _context.Messages.AddAsync(info);
                    #endregion

                    #region --add image to group--
                    if (request.ImageFile != null)
                    {
                        var upRes = await request.ImageFile.UploadFileAsync("Images/ChatoomImage/", _environment);
                        if (!upRes.Status)
                        {
                            return new ResultDto()
                            {
                                Status = Common.Enums.ServiceStatus.SaveFileError,
                            };
                        }

                        ChatroomImage chatroomImage = new ChatroomImage()
                        {
                            Chatroom = newChatroom,
                            ChatroomId = newChatroom.Id,
                            ImageName = upRes.FileNameAddress,
                            InsertTime = DateTime.Now,
                        };

                        await _context.ChatroomImages.AddAsync(chatroomImage);
                    }
                    #endregion

                    #region --Add Creator To Group--

                    UserInChatroom userInChatroom = new UserInChatroom()
                    {
                        Chatroom = newChatroom,
                        ChatroomId = newChatroom.Id,
                        User = Creator,
                        UserId = request.UserId,
                    };

                    await _context.UserInChatrooms.AddAsync(userInChatroom);

                    #endregion

                    await _context.SaveChangesAsync();

                    return new ResultDto()
                    {
                        Status = Common.Enums.ServiceStatus.Success,
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

    public class RequestCreateNewGroupService
    {
        public long UserId { get; set; }
        public string GroupName { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
