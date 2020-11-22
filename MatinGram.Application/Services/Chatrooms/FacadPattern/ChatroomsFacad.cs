using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.FacadPattern
{
    public class ChatroomsFacad: IChatroomsFacad
    {
        private readonly IDataBaseContext _context;

        public ChatroomsFacad(IDataBaseContext context)
        {
            _context = context;
        }

    }
}
