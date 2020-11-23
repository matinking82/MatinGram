using MatinGram.Application.Services.Users.Commands.UserSignin;
using MatinGram.Application.Services.Users.Commands.UserSignup;
using MatinGram.Application.Services.Users.Queries.GetUserProfileById;
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


        IGetUserProfileById GetUserProfileById { get; }

    }
}
