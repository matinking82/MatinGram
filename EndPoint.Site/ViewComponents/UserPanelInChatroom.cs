using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site.ViewComponents
{
    public class UserPanelInChatroom : ViewComponent
    {




        public IViewComponentResult Invoke()
        {
            return View(viewName: "UserPanelInChatroom");
        }
    }
}
