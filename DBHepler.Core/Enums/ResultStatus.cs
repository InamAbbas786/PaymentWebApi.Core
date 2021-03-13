using System;
using System.Collections.Generic;
using System.Text;

namespace DBHepler.Core.Enums
{
    public enum ResultStatus
    {
        OK = 200,
        Warning = 300,
        BadRequest = 400,
        InternalServerError = 500,
        AlreadyExist = 600
    };
    public enum PaymentStatus
    {
        pending = 1,
        processed = 2,
        failed = 0
    }
}
