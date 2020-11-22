using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Domain.Entities.Common
{
    public class BaseEntity
    {
        public DateTime InsertTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
