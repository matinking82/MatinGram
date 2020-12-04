using MatinGram.Application.Services.Chatrooms.Queries.GetGroupDetailForProfile;
using MatinGram.ViewModels.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Chatrooms
{
    public record GroupDetailForProfileViewModel
    {
        public string ImageName { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<GroupMemberViewModel> Members { get; set; }
        public MemberLevel MyLevel { get; set; }
    }

}
