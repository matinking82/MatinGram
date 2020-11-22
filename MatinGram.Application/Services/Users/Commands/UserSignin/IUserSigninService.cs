using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
using MatinGram.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Users.Commands.UserSignin
{
    public interface IUserSigninService
    {
        ResultDto<ResultUserSignin> Execute(RequestUserSigninDto request);
    }

    public class UserSigninService : IUserSigninService
    {
        private readonly IDataBaseContext _context;
        public UserSigninService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultUserSignin> Execute(RequestUserSigninDto request)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.MobileNumber == request.MobileNumber && u.Password == request.Password.ToHashed());

                if (user == null)
                {
                    return new ResultDto<ResultUserSignin>()
                    {
                        Status = ServiceStatus.NotFound,
                    };
                }


                return new ResultDto<ResultUserSignin>()
                {
                    Status = ServiceStatus.Success,
                    Data = new ResultUserSignin()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        UserInRole = user.UserInRole,
                    }
                };


            }
            catch (Exception)
            {
                return new ResultDto<ResultUserSignin>()
                {
                    Status = ServiceStatus.SystemError
                };
            };
        }
    }
}

public record ResultUserSignin
{
    public string Name { get; set; }
    public UserInRole UserInRole { get; set; }
    public long Id { get; set; }
}

public record RequestUserSigninDto
{
    public string MobileNumber { get; set; }
    public string Password { get; set; }
}

