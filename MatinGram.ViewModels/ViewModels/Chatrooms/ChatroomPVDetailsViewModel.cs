using MatinGram.ViewModels.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Chatrooms
{
    public record ChatroomPVDetailsViewModel
    {
        public string ChatroomName { get; set; }
        public string ImageName { get; set; }
        public Guid ChatroomGuid { get; set; }
        public IEnumerable<PVMessageViewModel> Messages { get; set; }
    }

    

}
