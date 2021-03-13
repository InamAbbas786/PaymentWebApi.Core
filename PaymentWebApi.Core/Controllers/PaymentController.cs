using DBHepler.Core.Entities;
using DBHepler.Core.Generics;
using DBHepler.Core.Payments;
using DBHepler.Core.Payments.IPayments;
using DBHepler.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentWebApi.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICheapPaymentGateway CheapPayment;
        public PaymentController(PaymentProcessContext db)
        {
            CheapPayment = new CheapPaymentGateway(db);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ProcessPayment")]
        public Result ProcessPayment(ProcessPaymentView payment)
        {
            try
            {
                if (Validations.IsValidateExpiryDate(payment.ExpirationDate)) //Pass MM/yyyy
                {
                    if (Validations.IsValidateCvv(payment.SecurityCode)) //CVV
                    {
                        if (Validations.ValidateCreditCard(payment.CreditCardNumber)) //check card number is valid
                        {
                            ProcessPayment process = new ProcessPayment();
                            process.Amount = payment.Amount;
                            process.CardHolder = payment.CardHolder;
                            process.CreditCardNumber = payment.CreditCardNumber;
                            process.SecurityCode = payment.SecurityCode;
                            process.ExpirationDate = payment.ExpirationDate;
                            process.CreatedDate = DateTime.UtcNow;
                            if (payment.Amount < 20) // If the amount to be paid is less than £20, use ICheapPaymentGateway
                            {
                                var save = CheapPayment.CheapPaymentProcess(process);
                                if (save.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                {
                                    PaymentLogs paymentLogs = new PaymentLogs();
                                    paymentLogs.ActionID = process.PaymentID;
                                    paymentLogs.Action = "Save CheapPaymentProcess";
                                    paymentLogs.CreatedDate = DateTime.UtcNow;
                                    paymentLogs.PaymentMethod = save.Message;
                                    paymentLogs.Desc = "CheapPaymentProcess Process successfully";
                                    var log = CheapPayment.SaveProcessLogs(paymentLogs);
                                    PaymentState state = new PaymentState();
                                    state.NumberOfAttempt = 1;
                                    state.PaymentID = process.PaymentID;
                                    state.PaymentStates = DBHepler.Core.Enums.PaymentStatus.processed;
                                    var states = CheapPayment.SaveProcessState(state, process.CreditCardNumber);
                                    if (log.Status == DBHepler.Core.Enums.ResultStatus.OK && states.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                    {
                                        return new Result()
                                        {
                                            Data = "Payment is processed",
                                            Message = "OK",
                                            Status = DBHepler.Core.Enums.ResultStatus.OK
                                        };
                                    }
                                }
                            }
                            else if (payment.Amount > 20 && payment.Amount < 500) //If the amount to be paid is £21 - 500, use IExpensivePaymentGateway if available.Otherwise, retry only once with ICheapPaymentGateway.
                            {
                                var saveexpensive = CheapPayment.ExpensivePaymentProcess(process);
                                if (saveexpensive.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                {
                                    PaymentLogs paymentLogs = new PaymentLogs();
                                    paymentLogs.ActionID = process.PaymentID;
                                    paymentLogs.Action = "Save ExpensivePaymentProcess";
                                    paymentLogs.CreatedDate = DateTime.UtcNow;
                                    paymentLogs.PaymentMethod = saveexpensive.Message;
                                    paymentLogs.Desc = "ExpensivePaymentProcess Process successfully";
                                    var log = CheapPayment.SaveProcessLogs(paymentLogs);
                                    PaymentState state = new PaymentState();
                                    state.NumberOfAttempt = 1;
                                    state.PaymentID = process.PaymentID;
                                    state.PaymentStates = DBHepler.Core.Enums.PaymentStatus.processed;
                                    var states = CheapPayment.SaveProcessState(state, process.CreditCardNumber);
                                    if (log.Status == DBHepler.Core.Enums.ResultStatus.OK && states.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                    {
                                        return new Result()
                                        {
                                            Data = "Payment is processed",
                                            Message = "OK",
                                            Status = DBHepler.Core.Enums.ResultStatus.OK
                                        };
                                    }
                                }
                                else
                                {
                                    var savecheap = CheapPayment.CheapPaymentProcess(process);
                                    if (savecheap.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                    {
                                        PaymentLogs paymentLogs = new PaymentLogs();
                                        paymentLogs.ActionID = process.PaymentID;
                                        paymentLogs.Action = "Save CheapPaymentProcess";
                                        paymentLogs.CreatedDate = DateTime.UtcNow;
                                        paymentLogs.PaymentMethod = savecheap.Message;
                                        paymentLogs.Desc = "CheapPaymentProcess Process successfully";
                                        var log = CheapPayment.SaveProcessLogs(paymentLogs);
                                        PaymentState state = new PaymentState();
                                        state.NumberOfAttempt = 1;
                                        state.PaymentID = process.PaymentID;
                                        state.PaymentStates = DBHepler.Core.Enums.PaymentStatus.processed;
                                        var states = CheapPayment.SaveProcessState(state, process.CreditCardNumber);
                                        if (log.Status == DBHepler.Core.Enums.ResultStatus.OK && states.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                        {
                                            return new Result()
                                            {
                                                Data = "Payment is processed",
                                                Message = "OK",
                                                Status = DBHepler.Core.Enums.ResultStatus.OK
                                            };
                                        }
                                    }
                                }
                            }
                            else // If the amount is > £500, try only PremiumPaymentService and retry up to 3 times in case payment  does not get processed
                            {
                            TryThree:
                                var State = CheapPayment.GetState(DBHepler.Core.Enums.PaymentStatus.failed, process.CreditCardNumber);
                                var SavePremium = CheapPayment.PremiumPaymentService(process);
                                if (State.Data < 4 && SavePremium.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                {
                                    PaymentLogs paymentLogs = new PaymentLogs();
                                    paymentLogs.ActionID = process.PaymentID;
                                    paymentLogs.Action = "Save PremiumPaymentService";
                                    paymentLogs.CreatedDate = DateTime.UtcNow;
                                    paymentLogs.PaymentMethod = SavePremium.Message;
                                    paymentLogs.Desc = "PremiumPaymentService Process successfully";
                                    var log = CheapPayment.SaveProcessLogs(paymentLogs);
                                    PaymentState state = new PaymentState();
                                    state.NumberOfAttempt = 1;
                                    state.PaymentID = process.PaymentID;
                                    state.PaymentStates = DBHepler.Core.Enums.PaymentStatus.processed;
                                    var states = CheapPayment.SaveProcessState(state, process.CreditCardNumber);
                                    if (log.Status == DBHepler.Core.Enums.ResultStatus.OK && states.Status== DBHepler.Core.Enums.ResultStatus.OK)
                                    {
                                        return new Result()
                                        {
                                            Data = "Payment is processed",
                                            Message = "OK",
                                            Status = DBHepler.Core.Enums.ResultStatus.OK
                                        };
                                    }
                                }
                                else
                                {
                                    if (State.Data > 3)
                                    {
                                        return new Result()
                                        {
                                            Data = null,
                                            Message = "- The request should be validated before processed.",//- The request should be validated before processed.
                                            Status = DBHepler.Core.Enums.ResultStatus.Warning
                                        };
                                    }
                                    PaymentLogs paymentLogs = new PaymentLogs();
                                    paymentLogs.ActionID = process.PaymentID;
                                    paymentLogs.Action = "Save PremiumPaymentService";
                                    paymentLogs.CreatedDate = DateTime.UtcNow;
                                    paymentLogs.PaymentMethod = SavePremium.Message;
                                    paymentLogs.Desc = "PremiumPaymentService Process successfully";
                                    var log = CheapPayment.SaveProcessLogs(paymentLogs);
                                    PaymentState state = new PaymentState();
                                    state.NumberOfAttempt = 1;
                                    state.PaymentID = process.PaymentID;
                                    state.PaymentStates = DBHepler.Core.Enums.PaymentStatus.failed;
                                    var states = CheapPayment.SaveProcessState(state, process.CreditCardNumber);
                                    if (log.Status == DBHepler.Core.Enums.ResultStatus.OK && states.Status == DBHepler.Core.Enums.ResultStatus.OK)
                                    {
                                        goto TryThree;
                                    }
                                }
                            }
                            return new Result()
                            {
                                Data = null,
                                Message = "Your credit card number is invalid",
                                Status = DBHepler.Core.Enums.ResultStatus.Warning
                            };
                        }
                        else
                        {
                            return new Result()
                            {
                                Data = null,
                                Message = "Your credit card number is invalid",
                                Status = DBHepler.Core.Enums.ResultStatus.Warning
                            };
                        }
                    }
                    else
                    {
                        return new Result()
                        {
                            Data = null,
                            Message = "Your CVV is invalid",
                            Status = DBHepler.Core.Enums.ResultStatus.Warning
                        };
                    }
                }
                else
                {
                    return new Result()
                    {
                        Data = null,
                        Message = "Your credit card has been expired",
                        Status = DBHepler.Core.Enums.ResultStatus.Warning
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    Data = null,
                    Message = "Internal Server Error",
                    Status = DBHepler.Core.Enums.ResultStatus.InternalServerError
                };
            }
        }
    }
}
