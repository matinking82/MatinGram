using MatinGram.Application.Services.Chatrooms.Queries.GetGroupDetailForProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Users
{
    public record GroupMemberViewModel
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string HashKey { get; set; }
        public MemberLevel MemberLevel { get; set; }
    }
}
