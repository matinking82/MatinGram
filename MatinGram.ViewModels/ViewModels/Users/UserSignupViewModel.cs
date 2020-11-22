using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Users
{
    public record UserSignupViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.ImageUrl)]
        public IFormFile ImageFile { get; set; }
    }
}
