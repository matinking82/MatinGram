using MatinGram.Common.Enums;
using MatinGram.Domain.Entities.Common;
using MatinGram.Domain.Entities.Messages;
using MatinGram.Domain.Entities.Relations;
using MatinGram.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Domain.Entities.Chatrooms
{
    public class Chatroom : BaseEntity
    {
        public int Id { get; set; }

        public ChatroomType ChatroomType { get; set; }

        public Guid Guid { get; set; }


        public virtual User Creator { get; set; }
        public long CreatorId { get; set; }


        public virtual IEnumerable<ChatroomImage> ChatroomImages { get; set; }

        public virtual IEnumerable<Message> Messages { get; set; }

        public virtual IEnumerable<AdminInChatroom> AdminInChatroom { get; set; }

        public virtual IEnumerable<UserInChatroom> UserInChatrooms { get; set; }


    }
}
