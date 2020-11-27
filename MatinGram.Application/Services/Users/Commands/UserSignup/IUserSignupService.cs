using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using MatinGram.Common.Enums;
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
        Task<ResultDto<ResultUserSignup>> Execute(RequestUserSignupDto request);
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
        public async Task<ResultDto<ResultUserSignup>> Execute(RequestUserSignupDto request)
        {
            return await Task.Run(async () =>
            {

                if (_context.Users.Any(u=> u.MobileNumber==request.MobileNumber))
                {
                    return new ResultDto<ResultUserSignup>()
                    {
                        Status = ServiceStatus.Error,
                        Message = "این شماره قبلا ثبت شده است!!"
                    };
                }


                try
                {
                    User newUser = new User()
                    {
                        MobileNumber = request.MobileNumber,
                        Name = request.Name,
                        Password = await request.Password.ToHashedAsync(),
                        InsertTime = DateTime.Now,
                        LastOnline = DateTime.Now,
                        UserInRole = UserInRole.User,
                        HashKey = await Guid.NewGuid().ToString().ToHashedAsync()
                    };

                    await _context.Users.AddAsync(newUser);

                    #region --Save Image--
                    if (request.ImageFile != null)
                    {
                        var upResult = await request.ImageFile.UploadFileAsync("Images/UserImages/", _environment);
                        if (upResult.Status)
                        {
                            var newImage = new UserImage()
                            {
                                ImageName = upResult.FileNameAddress,
                                User = newUser,
                                UserId = newUser.Id,
                                InsertTime = DateTime.Now,
                            };

                            await _context.UserImages.AddAsync(newImage);
                        }
                        else
                        {
                            return new ResultDto<ResultUserSignup>()
                            {
                                Status = ServiceStatus.SaveFileError,
                            };
                        }
                    }
                    #endregion



                    await _context.SaveChangesAsync();

                    return new ResultDto<ResultUserSignup>()
                    {
                        Data = new ResultUserSignup()
                        {
                            UserId = newUser.Id,
                        },
                        Status = ServiceStatus.Success
                    };


                }
                catch (Exception)
                {
                    return new ResultDto<ResultUserSignup>()
                    {
                        Status = ServiceStatus.SystemError
                    };
                }
            });
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
