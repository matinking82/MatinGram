using MatinGram.Application.Services.Chatrooms.Commands.CreateNewChatroomPV;
using MatinGram.Application.Services.Chatrooms.Commands.CreateNewGroup;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByGuid;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomsByUserId;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomTypeByGuid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Interfaces.FacadPatterns
{
    public interface IChatroomsFacad
    {
        IGetChatroomsByUserIdService GetChatroomsByUserIdService { get; }
        IGetChatroomDetailByUsernameService GetChatroomDetailByUsernameService { get; }
        IGetChatroomDetailByGuid GetChatroomDetailByGuid { get; }
        IGetChatroomTypeByGuidService GetChatroomTypeByGuidService { get; }

        ICreateNewChatroomPVService CreateNewChatroomPVService { get; }
        ICreateNewGroupService CreateNewGroupService { get; }
    }
}
