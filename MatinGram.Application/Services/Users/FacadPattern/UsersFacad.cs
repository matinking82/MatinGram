using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Users.FacadPattern
{
    public class UsersFacad: IUsersFacad
    {
        private readonly IDataBaseContext _context;

        public UsersFacad(IDataBaseContext context)
        {
            _context = context;
        }

    }
}
