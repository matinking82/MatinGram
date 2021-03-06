﻿using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Users.Commands.UserSignin;
using MatinGram.Application.Services.Users.Commands.UserSignup;
using MatinGram.Application.Services.Users.Queries.GetUserProfileById;
using MatinGram.Application.Services.Users.Queries.GetUserPublicProfileByUserId;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Users.FacadPattern
{
    public class UsersFacad : IUsersFacad
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public UsersFacad(IDataBaseContext context, IHostingEnvironment environment)
        {
            _environment = environment;
            _context = context;
        }

        
        private IUserSignupService _userSignupService;
        public IUserSignupService UserSignupService
        {
            get
            {
                if (_userSignupService == null)
                {
                    _userSignupService = new UserSignupService(_context, _environment);
                }

                return _userSignupService;
            }
        }

        private IUserSigninService _userSigninService;
        public IUserSigninService UserSigninService
        {
            get
            {
                if (_userSigninService == null)
                {
                    _userSigninService = new UserSigninService(_context);
                }

                return _userSigninService;
            }
        }

        private IGetUserProfileById _getUserProfileById;
        public IGetUserProfileById GetUserProfileById 
        { 
            get 
            {
                if (_getUserProfileById == null)
                {
                    _getUserProfileById = new GetUserProfileById(_context);
                }

                return _getUserProfileById;
            } 
        }

        private IGetUserPublicProfileByUserIdService _getUserPublicProfileByUserIdService;
        public IGetUserPublicProfileByUserIdService GetUserPublicProfileByUserIdService 
        { 
            get 
            {
                if (_getUserPublicProfileByUserIdService == null)
                {
                    _getUserPublicProfileByUserIdService = new GetUserPublicProfileByUserIdService(_context);
                }

                return _getUserPublicProfileByUserIdService;
            } 
        }

    }
}
