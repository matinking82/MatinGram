using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Users.Queries.GetUserProfileById
{
    public interface IGetUserProfileById
    {
        Task<ResultDto<ResultGetUserProfileByIdDto>> Execute(long UserId);
    }

    public class GetUserProfileById : IGetUserProfileById
    {
        private readonly IDataBaseContext _context;
        public GetUserProfileById(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ResultGetUserProfileByIdDto>> Execute(long UserId)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == UserId);

                    if (user == null)
                    {
                        return new ResultDto<ResultGetUserProfileByIdDto>()
                        {
                            Status = ServiceStatus.NotFound,
                        };
                    }


                    ResultGetUserProfileByIdDto Data = new ResultGetUserProfileByIdDto()
                    {
                        Bio = user.Bio,
                        Mobile = user.MobileNumber,
                        Name = user.Name,
                        UserId = user.Id,
                        Username = user.Username,
                    };

                    #region --Find Image--
                    var userImage = _context.UserImages
                    .Where(u => u.UserId == UserId)
                    .ToList()
                    .LastOrDefault();

                    if (userImage != null)
                    {
                        Data.ImageName = userImage.ImageName;
                    }
                    else
                    {
                        Data.ImageName = "/Images/UserImages/Default.png";
                    }
                    #endregion

                    return new()
                    {
                        Data = Data,
                        Status = ServiceStatus.Success
                    };

                }
                catch (Exception)
                {
                    return new ResultDto<ResultGetUserProfileByIdDto>()
                    {
                        Status = ServiceStatus.SystemError,
                    };
                }
            });
        }
    }

    public record ResultGetUserProfileByIdDto
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Bio { get; set; }
        public string ImageName { get; set; }
        public string Username { get; set; }
    }
}
