using MatinGram.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EndPoint.Site.Utilities
{
    public static class ClaimUtilities
    {
        public static long GetUserId(this ClaimsPrincipal User)
        {
            return long.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public static UserInRole GetUserRole(this ClaimsPrincipal User)
        {
            return (UserInRole)(int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Role).Value));
        }

        public static string GetMobile(this ClaimsPrincipal User)
        {
            return ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.MobilePhone).Value;
        }

        public static string GetName(this ClaimsPrincipal User)
        {
            return ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
        }
    }
}
