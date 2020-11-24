using MatinGram.Common.Enums;
using MatinGram.Domain.Entities.Chatrooms;
using MatinGram.Domain.Entities.Common;
using MatinGram.Domain.Entities.Messages;
using MatinGram.Domain.Entities.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string MobileNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string HashKey { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; }

        [MinLength(10)]
        [MaxLength(100)]
        public string Bio { get; set; }

        public DateTime LastOnline { get; set; }

        public UserInRole UserInRole { get; set; }


        public virtual IEnumerable<UserImage> UserImages { get; set; }

        public virtual IEnumerable<Chatroom> CreatedChatrooms { get; set; }

        public virtual IEnumerable<Message> Messages { get; set; }

        public virtual IEnumerable<AdminInChatroom> AdminInChatroom { get; set; }

        public virtual IEnumerable<UserInChatroom> UserInChatrooms { get; set; }
    }

}
