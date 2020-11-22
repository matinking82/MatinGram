using EndPoint.Site.Utilities;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Users.Commands.UserSignup;
using MatinGram.Common.Enums;
using MatinGram.ViewModels.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IUsersFacad _usersFacad;
        public AuthenticationController(IUsersFacad usersFacad)
        {
            _usersFacad = usersFacad;
        }

        [HttpGet]
        [Route("/Signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [Route("/Signup")]
        public IActionResult Signup([Bind("MobileNumber,Name,Password,ConfirmPassword,ImageFile")] UserSignupViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != user.ConfirmPassword)
                {
                    ViewBag.Message = "کلمه عبور و تکرار آن یکسان نیست";
                    return View("ShowMessage");
                }

                RequestUserSignupDto request = new RequestUserSignupDto()
                {
                    ImageFile = user.ImageFile,
                    MobileNumber = user.MobileNumber,
                    Name = user.Name,
                    Password = user.Password
                };
                var result = _usersFacad.UserSignupService.Execute(request);


                if (result.Status != ServiceStatus.Success)
                {
                    switch (result.Status)
                    {
                        case ServiceStatus.SystemError:
                            ViewBag.Message = "مشکلی در سیستم پیش آمد";
                            break;

                        case ServiceStatus.NotFound:
                            return NotFound();

                        case ServiceStatus.SaveFileError:
                            ViewBag.Message = "مشکلی در ذخیره فایل پیش آمد";
                            break;

                        case ServiceStatus.Error:
                            ViewBag.Message = result.Message;
                            break;
                    }
                    return View("ShowMessage");
                }

                //Login
                HttpContext.LoginToSite(result.Data.UserId, user.MobileNumber, user.Name, UserInRole.User);
                /////

                ViewBag.Message = "حساب شما با موفقیت ایجاد شد";
                return View("ShowMessage");

            }
            return View(user);
        }


        [HttpGet]
        [Route("/Signin")]
        public IActionResult Signin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        [Route("/Signin")]
        public IActionResult Signin([Bind("MobileNumber,Password")] UserSigninViewModel user)
        {
            return View(user);
        }


    }
}
