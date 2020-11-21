namespace MatinGram.Domain.Entities.Chatrooms
{
    public class ChatroomImage
    {
        public long Id { get; set; }

        public string ImageName { get; set; }


        public virtual Chatroom Chatroom { get; set; }
        public int ChatroomId { get; set; }

    }
}
