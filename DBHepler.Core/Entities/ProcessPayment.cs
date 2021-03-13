using DBHepler.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DBHepler.Core.Entities
{
    public class ProcessPayment
    {
        public ProcessPayment()
        {
            CreatedDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required"),CreditCard(ErrorMessage = "Credit Card Number is invalid")]
        public string CreditCardNumber { get; set; }
        [Required(ErrorMessage = "Card Holder Name is required")]
        public string CardHolder { get; set; }
        [Required(ErrorMessage = "Expiry Date is required")]
        public string ExpirationDate { get; set; }
        [StringLength(3)]
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
