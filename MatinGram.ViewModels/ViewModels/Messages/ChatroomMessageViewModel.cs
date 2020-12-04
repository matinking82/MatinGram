using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Messages
{
    public record ChatroomMessageViewModel
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsMe { get; set; }
        public long MessageId { get; set; }

        public string ImageName { get; set; }
        public string SenderName { get; set; }
    }
}
