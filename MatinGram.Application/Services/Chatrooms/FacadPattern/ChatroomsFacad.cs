using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Chatrooms.Commands.ChangeJoinLinkGuid;
using MatinGram.Application.Services.Chatrooms.Commands.CreateNewChatroomPV;
using MatinGram.Application.Services.Chatrooms.Commands.CreateNewGroup;
using MatinGram.Application.Services.Chatrooms.Commands.JoinToChatWithLink;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByGuid;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomGuidByJoinGuid;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomJoinLinkByChatroomGuid;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomsByUserId;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomTypeByGuid;
using MatinGram.Application.Services.Chatrooms.Queries.GetGroupDetailForProfile;
using MatinGram.Application.Services.Chatrooms.Queries.GetUserIdByPVGuid;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _environment;

        public ChatroomsFacad(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
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


        private ICreateNewGroupService _createNewGroupService;
        public ICreateNewGroupService CreateNewGroupService 
        { 
            get 
            {
                if (_createNewGroupService == null)
                {
                    _createNewGroupService = new CreateNewGroupService(_context, _environment);
                }
    
                return _createNewGroupService;
            } 
        }

        private IGetChatroomTypeByGuidService _getChatroomTypeByGuidService;
        public IGetChatroomTypeByGuidService GetChatroomTypeByGuidService 
        { 
            get 
            {
                if (_getChatroomTypeByGuidService == null)
                {
                    _getChatroomTypeByGuidService = new GetChatroomTypeByGuidService(_context);
                }

                return _getChatroomTypeByGuidService;
            } 
        }

        private IGetUserIdByPVGuidService _getUserIdByPVGuidService;
        public IGetUserIdByPVGuidService GetUserIdByPVGuidService 
        { 
            get 
            {
                if (_getUserIdByPVGuidService == null)
                {
                    _getUserIdByPVGuidService = new GetUserIdByPVGuidService(_context);
                }

                return _getUserIdByPVGuidService;
            } 
        }

        private IGetGroupDetailForProfileService _getGroupDetailForProfileService;
        public IGetGroupDetailForProfileService GetGroupDetailForProfileService 
        { 
            get 
            {
                if (_getGroupDetailForProfileService == null)
                {
                    _getGroupDetailForProfileService = new GetGroupDetailForProfileService(_context);
                }

                return _getGroupDetailForProfileService;
            } 
        }

        private IGetChatroomJoinLinkByChatroomGuidService _getChatroomJoinLinkByChatroomGuidService;
        public IGetChatroomJoinLinkByChatroomGuidService GetChatroomJoinLinkByChatroomGuidService 
        { 
            get 
            {
                if (_getChatroomJoinLinkByChatroomGuidService == null)
                {
                    _getChatroomJoinLinkByChatroomGuidService = new GetChatroomJoinLinkByChatroomGuidService(_context);
                }

                return _getChatroomJoinLinkByChatroomGuidService;
            } 
        }

        private IChangeJoinLinkGuidService _changeJoinLinkGuidService;
        public IChangeJoinLinkGuidService ChangeJoinLinkGuidService 
        { 
            get 
            {
                if (_changeJoinLinkGuidService == null)
                {
                    _changeJoinLinkGuidService = new ChangeJoinLinkGuidService(_context);
                }

                return _changeJoinLinkGuidService;
            } 
        }

        private IJoinToChatWithLinkService _joinToChatWithLinkService;
        public IJoinToChatWithLinkService JoinToChatWithLinkService 
        { 
            get 
            {
                if (_joinToChatWithLinkService == null)
                {
                    _joinToChatWithLinkService = new JoinToChatWithLinkService(_context);
                }

                return _joinToChatWithLinkService;
            } 
        }

        private IGetChatroomGuidByJoinGuidService _getChatroomGuidByJoinGuidService;
        public IGetChatroomGuidByJoinGuidService GetChatroomGuidByJoinGuidService 
        { 
            get 
            {
                if (_getChatroomGuidByJoinGuidService == null)
                {
                    _getChatroomGuidByJoinGuidService = new GetChatroomGuidByJoinGuidService(_context);
                }

                return _getChatroomGuidByJoinGuidService;
            } 
        }
    }
}
