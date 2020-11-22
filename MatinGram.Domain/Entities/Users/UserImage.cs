using MatinGram.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace MatinGram.Domain.Entities.Users
{
    public class UserImage:BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public string ImageName { get; set; }


        public virtual User User { get; set; }
        public long UserId { get; set; }

    }

}
