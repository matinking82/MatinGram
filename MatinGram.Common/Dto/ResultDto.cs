using MatinGram.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Common.Dto
{
    public record ResultDto
    {
        public ServiceStatus Status { get; set; }
        public string Message { get; set; }
    }

    public record ResultDto<T>
    {
        public T Data { get; set; }
        public ServiceStatus Status { get; set; }
        public string Message { get; set; }
    }
}
