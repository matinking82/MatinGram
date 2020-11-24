﻿using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomDetailByUsername;
using MatinGram.Application.Services.Chatrooms.Queries.GetChatroomsByUserId;
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


    }
}
