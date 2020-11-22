using MatinGram.Domain.Entities.Common;

namespace MatinGram.Domain.Entities.Chatrooms
{
    public class ChatroomImage : BaseEntity
    {
        public long Id { get; set; }

        public string ImageName { get; set; }


        public virtual Chatroom Chatroom { get; set; }
        public int ChatroomId { get; set; }

    }
}
