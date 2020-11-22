using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Users
{
    public record UserSigninViewModel
    {

        public string MobileNumber { get; set; }
        public string Password { get; set; }

    }
}
