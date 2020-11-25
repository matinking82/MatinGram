using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Messages
{
    public record AddNewMessageViewModel
    {
        public string Text { get; set; }
        public Guid Guid { get; set; }
    }
}
