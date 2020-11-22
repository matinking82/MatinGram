using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Utilities;
using MatinGram.Domain.Entities.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Users.Commands.UserSignup
{
    public interface IUserSignupService
    {
        ResultDto<ResultUserSignup> Execute(RequestUserSignupDto request);
    }

    public class UserSignupService : IUserSignupService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public UserSignupService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _environment = environment;
            _context = context;
        }
        public ResultDto<ResultUserSignup> Execute(RequestUserSignupDto request)
        {
            try
            {
                User newUser = new User()
                {
                    MobileNumber = request.MobileNumber,
                    Name = request.Name,
                    Password = request.Password.ToHashed(),
                    InsertTime = DateTime.Now,
                    LastOnline = DateTime.Now
                };

                _context.Users.Add(newUser);

                #region --Save Image--
                if (request.ImageFile != null)
                {
                    var upResult = request.ImageFile.UploadFile("Images/UserImages", _environment);
                    if (upResult.Status)
                    {
                        var newImage = new UserImage()
                        {
                            ImageName = upResult.FileNameAddress,
                            User = newUser,
                            UserId = newUser.Id,
                            InsertTime = DateTime.Now,
                        };

                        _context.UserImages.Add(newImage);
                    }
                    else
                    {
                        return new ResultDto<ResultUserSignup>()
                        {
                            Status = Common.Enums.ServiceStatus.SaveFileError,
                        };
                    }
                }
                #endregion



                _context.SaveChanges();

                return new ResultDto<ResultUserSignup>()
                {
                    Data = new ResultUserSignup()
                    {
                        UserId = newUser.Id,
                    },
                    Status = Common.Enums.ServiceStatus.Success
                };


            }
            catch (Exception)
            {
                return new ResultDto<ResultUserSignup>()
                {
                    Status = Common.Enums.ServiceStatus.SystemError
                };
            }
        }
    }

    public record ResultUserSignup
    {
        public long UserId { get; set; }
    }

    public record RequestUserSignupDto
    {
        public string MobileNumber { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
