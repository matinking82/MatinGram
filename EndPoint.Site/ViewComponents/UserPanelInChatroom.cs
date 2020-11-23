using MatinGram.Application.Interfaces.FacadPatterns;
using EndPoint.Site.Utilities;
using MatinGram.ViewModels.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.ViewComponents
{
    public class UserPanelInChatroom : ViewComponent
    {
        private readonly IUsersFacad _usersFacad;
        public UserPanelInChatroom(IUsersFacad usersFacad)
        {
            _usersFacad = usersFacad;
        }


        public IViewComponentResult Invoke()
        {

            long UserId = HttpContext.User.GetUserId();

            var result = _usersFacad.GetUserProfileById.Execute(UserId);

            return View(viewName: "UserPanelInChatroom",model: new UserProfileViewModel()
            {
                UserId = result.Data.UserId,
                Bio = result.Data.Bio,
                ImageName = result.Data.ImageName,
                Mobile = result.Data.Mobile,
                Name = result.Data.Name,
                Username = result.Data.Username,
            });
        }
    }
}
