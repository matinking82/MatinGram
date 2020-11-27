using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Users.Queries.GetUserPublicProfileByUserId
{
    public interface IGetUserPublicProfileByUserIdService
    {
        Task<ResultDto<ResultGetUserPublicProfileByIdDto>> ExecuteAsync(long UserId);
    }

    public class GetUserPublicProfileByUserIdService : IGetUserPublicProfileByUserIdService
    {
        private readonly IDataBaseContext _context;
        public GetUserPublicProfileByUserIdService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<ResultGetUserPublicProfileByIdDto>> ExecuteAsync(long UserId)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == UserId);

                    if (user == null)
                    {
                        return new ResultDto<ResultGetUserPublicProfileByIdDto>()
                        {
                            Status = ServiceStatus.NotFound,
                        };
                    }


                    ResultGetUserPublicProfileByIdDto Data = new ResultGetUserPublicProfileByIdDto()
                    {
                        Bio = user.Bio,
                        Name = user.Name,
                        UserHaskKey = user.HashKey,
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
                        Data.ImageName = "Images/UserImages/Default.png";
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
                    return new ResultDto<ResultGetUserPublicProfileByIdDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }
    }

    public record ResultGetUserPublicProfileByIdDto
    {
        public string UserHaskKey { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ImageName { get; set; }
        public string Username { get; set; }
    }
}
