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
        ResultDto<ResultGetUserProfileByIdDto> Execute(long UserId);
    }

    public class GetUserProfileById : IGetUserProfileById
    {
        private readonly IDataBaseContext _context;
        public GetUserProfileById(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetUserProfileByIdDto> Execute(long UserId)
        {
            try
            {
                var user = _context.Users
                    .Include(u => u.UserImages)
                    .FirstOrDefault(u => u.Id == UserId);

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
                    ImageName = user.UserImages.Count() > 0 ? user.UserImages.First().ImageName : "/Images/UserImages/Default.png",
                    Mobile = user.MobileNumber,
                    Name = user.Name,
                    UserId = user.Id,
                    Username = user.Username,
                };

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
