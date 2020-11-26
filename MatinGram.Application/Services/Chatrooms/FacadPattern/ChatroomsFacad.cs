using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.Commands.CreateNewChatroomPV;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByGuid;
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
                    _getChatroomsByUserIdService = new NewGetChatroomsByUserIdService(_context);
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
                    _getChatroomDetailByUsernameService = new NewGetChatroomDetailByUsernameService(_context);
                }

                return _getChatroomDetailByUsernameService;
            } 
        }

        private ICreateNewChatroomPVService _createNewChatroomPVService;
        public ICreateNewChatroomPVService CreateNewChatroomPVService 
        { 
            get 
            {
                if (_createNewChatroomPVService == null)
                {
                    _createNewChatroomPVService = new CreateNewChatroomPVService(_context);
                }

                return _createNewChatroomPVService;
            } 
        }


        private IGetChatroomDetailByGuid _getChatroomDetailByGuid;
        public IGetChatroomDetailByGuid GetChatroomDetailByGuid 
        { 
            get 
            {
                if (_getChatroomDetailByGuid == null)
                {
                    _getChatroomDetailByGuid = new GetChatroomDetailByGuid(_context);
                }

                return _getChatroomDetailByGuid;
            } 
        }
    }
}
