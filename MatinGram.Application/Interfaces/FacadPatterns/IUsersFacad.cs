using MatinGram.Application.Services.Users.Commands.UserSignin;
using MatinGram.Application.Services.Users.Commands.UserSignup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Interfaces.FacadPatterns
{
    public interface IUsersFacad
    {
        IUserSignupService UserSignupService { get; }
        IUserSigninService UserSigninService { get; }
    }
}
