using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Users
{
    public record UserPublicProfileViewModel
    {
        public string UserHaskKey { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ImageName { get; set; }
        public string Username { get; set; }
    }
}
