﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Common.Enums
{
    public enum ServiceStatus
    {
        Success,
        SystemError,
        NotFound,
        InputParametersError,
        AccessDenied,
        SaveFileError,
        Error
    }
}
