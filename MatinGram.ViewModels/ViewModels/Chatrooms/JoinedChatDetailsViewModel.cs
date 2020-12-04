using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Chatrooms
{
    public record JoinedChatDetailsViewModel
    {
        public string LastMessage { get; set; }
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public Guid Guid { get; set; }
    }
}
