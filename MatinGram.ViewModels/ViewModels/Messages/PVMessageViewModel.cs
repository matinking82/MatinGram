using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.ViewModels.ViewModels.Messages
{
    public record PVMessageViewModel
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsMe { get; set; }
        public long MessageId { get; set; }
    }
}
