using System.ComponentModel.DataAnnotations;

namespace MatinGram.Domain.Entities.Users
{
    public class UserImage
    {
        [Key]
        public long Id { get; set; }

        public string ImageName { get; set; }


        public virtual User User { get; set; }
        public long UserId { get; set; }

    }

}
