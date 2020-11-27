using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Chatrooms
{
    public record CreateNewGroupViewModel
    {
        public string GroupName { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
