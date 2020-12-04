using MatinGram.Common.Enums;
using MatinGram.ViewModels.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Chatrooms
{
    public record ChatroomDetailsViewModel
    {
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public Guid ChatroomGuid { get; set; }
        public ChatroomType Type { get; set; }
        public IEnumerable<ChatroomMessageViewModel> Messages { get; set; }
    }

    
}
