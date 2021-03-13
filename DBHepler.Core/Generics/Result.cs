using DBHepler.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBHepler.Core.Generics
{
    public struct Result
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
