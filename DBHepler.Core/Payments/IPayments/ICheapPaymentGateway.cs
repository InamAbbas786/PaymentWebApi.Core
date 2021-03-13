using DBHepler.Core.Entities;
using DBHepler.Core.Enums;
using DBHepler.Core.Generics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBHepler.Core.Payments.IPayments
{
    public interface ICheapPaymentGateway
    {
        Result CheapPaymentProcess(ProcessPayment payment);
        Result SaveProcessState(PaymentState paymentState,string CardNo);
        Result SaveProcessLogs(PaymentLogs Logs);
        Result GetState(PaymentStatus paymentState, string CardNo);
        Result PremiumPaymentService(ProcessPayment payment);
        Result ExpensivePaymentProcess(ProcessPayment payment);
    }
}
