using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomsByUserId;
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


        private IGetChatroomsByUserIdService _getChatroomsByUserIdService;
        public IGetChatroomsByUserIdService GetChatroomsByUserIdService 
        { 
            get 
            {
                if (_getChatroomsByUserIdService == null)
                {
                    _getChatroomsByUserIdService = new GetChatroomsByUserIdService(_context);
                }

                return _getChatroomsByUserIdService;
            } 
        }

        private IGetChatroomDetailByUsernameService _getChatroomDetailByUsernameService;
        public IGetChatroomDetailByUsernameService GetChatroomDetailByUsernameService 
        { 
            get 
            {
                if (_getChatroomDetailByUsernameService == null)
                {
                    _getChatroomDetailByUsernameService = new GetChatroomDetailByUsernameService(_context);
                }

                return _getChatroomDetailByUsernameService;
            } 
        }

    }
}
