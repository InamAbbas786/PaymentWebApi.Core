using DBHepler.Core.Entities;
using DBHepler.Core.Enums;
using DBHepler.Core.Generics;
using DBHepler.Core.Payments.IPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHepler.Core.Payments
{
    public class CheapPaymentGateway : ICheapPaymentGateway
    {
        private PaymentProcessContext _Db = null;
        public CheapPaymentGateway(PaymentProcessContext db)
        {
            _Db = db;
        }
        #region PayementProcessSave
        public Result CheapPaymentProcess(ProcessPayment payment)
        {
            try
            {
                 _Db.ProcessPayments.Add(payment);
                _Db.SaveChanges();
                return new Result()
                {
                    Data = payment,
                    Message = "CheapPaymentProcess",
                    Status = Enums.ResultStatus.OK
                };
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Exception",
                    Status = Enums.ResultStatus.Warning
                };
            }
        }
        public Result ExpensivePaymentProcess(ProcessPayment payment)
        {
            try
            {
                _Db.ProcessPayments.Add(payment);
                _Db.SaveChanges();
                return new Result()
                {
                    Data = payment,
                    Message = "ExpensivePaymentProcess",
                    Status = Enums.ResultStatus.OK

                };
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Exception",
                    Status = Enums.ResultStatus.Warning
                };
            }
        }
        public Result PremiumPaymentService(ProcessPayment payment)
        {
            try
            {
                _Db.ProcessPayments.Add(payment);
                _Db.SaveChanges();
                return new Result()
                {
                    Data = payment,
                    Message = "PremiumPaymentService",
                    Status = Enums.ResultStatus.OK
                };
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Exception",
                    Status = Enums.ResultStatus.Warning
                };
            }
        }
        #endregion
        #region PaymentState
        public Result SaveProcessState(PaymentState paymentState, string CardNo)
        {
            try
            {
                var payment = _Db.PaymentStates.Where(x => x.ProcessPayment.CreditCardNumber == CardNo && x.PaymentStates == paymentState.PaymentStates).SingleOrDefault();
                if (payment == null)
                {
                    _Db.PaymentStates.Add(paymentState);
                    _Db.SaveChanges();
                }
                else
                {
                    payment.NumberOfAttempt = payment.NumberOfAttempt + 1;
                    payment.PaymentStates = paymentState.PaymentStates;
                    _Db.SaveChanges();
                }
                return new Result()
                {
                    Data = paymentState,
                    Message = "CheapPaymentProcess",
                    Status = Enums.ResultStatus.OK
                };
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Exception",
                    Status = Enums.ResultStatus.Warning
                };
            }
        }


        public Result GetState(PaymentStatus paymentState, string CardNo)
        {
            try
            {
                var payment = _Db.PaymentStates.Where(x => x.ProcessPayment.CreditCardNumber == CardNo && x.PaymentStates == paymentState).SingleOrDefault();
                if (payment == null)
                {
                    return new Result()
                    {
                        Data = 0,
                        Message = "",
                        Status = Enums.ResultStatus.OK
                    };
                }
                else
                {
                    return new Result()
                    {
                        Data = payment.NumberOfAttempt,
                        Message = "",
                        Status = Enums.ResultStatus.OK
                    };
                }

            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Exception",
                    Status = Enums.ResultStatus.Warning
                };
            }
        }
        #endregion


        #region PaymentLog
        public Result SaveProcessLogs(PaymentLogs Logs)
        {
            try
            {
                _Db.PaymentLogs.Add(Logs);
                _Db.SaveChanges();
                return new Result()
                {
                    Data = Logs,
                    Message = "SaveProcessLogs",
                    Status = Enums.ResultStatus.OK
                };
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Exception",
                    Status = Enums.ResultStatus.Warning
                };
            }
        }
        #endregion
    }
}
