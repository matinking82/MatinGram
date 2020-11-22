using MatinGram.ViewModels.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.Controllers
{
    public class AuthenticationController : Controller
    {
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
            return View(user);
        }


        [HttpGet]
        [Route("/Signin")]
        public IActionResult Signin()
        {
            return View();
        }
    }
}
