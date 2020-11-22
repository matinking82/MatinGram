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
        public IActionResult Signup()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }
    }
}
