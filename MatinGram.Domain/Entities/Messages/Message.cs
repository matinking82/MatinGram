using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Domain.Entities.Messages
{
    public class Message
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Text { get; set; }

        [Required]
        public DateTime SendDate { get; set; }
        public DateTime UpdateTime { get; set; }

        public string ImageName { get; set; }

        public virtual User Sender { get; set; }
        public long SenderId { get; set; }

        public virtual Chatroom Chatroom { get; set; }
        public int ChatroomID { get; set; }

        public MessageType MessageType { get; set; }

        #region --Reply--
        //public virtual Message Reply { get; set; }
        //public long ReplyId { get; set; }
        #endregion
    }

    public enum MessageType
    {
        Massage,
        Info
    }
}
