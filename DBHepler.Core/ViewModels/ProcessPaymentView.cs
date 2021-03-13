using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBHepler.Core.ViewModels
{
   public class ProcessPaymentView
    {
        [Required(ErrorMessage = "Credit Card Number is required"), CreditCard(ErrorMessage = "Credit Card Number is invalid")]
        public string CreditCardNumber { get; set; }
        [Required(ErrorMessage = "Card Holder Name is required")]
        public string CardHolder { get; set; }
        [Required(ErrorMessage = "Expiry Date is required")]
        public string ExpirationDate { get; set; } //Pass MM/yyyy
        [StringLength(3)]
        public string SecurityCode { get; set; }
        [Range(0, 9999999999999999.99)]
        public decimal Amount { get; set; }
    }
}
