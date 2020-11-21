using MatinGram.Common.Enums;
using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Domain.Entities.Relations
{
    public class AdminInChatroom
    {
        public long Id { get; set; }

        public virtual User User { get; set; }
        public long UserId { get; set; }

        public virtual Chatroom Chatroom { get; set; }
        public int ChatroomId { get; set; }

        public AdminInChatroomRoles Role { get; set; }
    }
}
