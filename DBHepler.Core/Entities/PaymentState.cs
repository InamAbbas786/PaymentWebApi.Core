using DBHepler.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DBHepler.Core.Entities
{
    public class PaymentState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateID { get; set; }
        public int NumberOfAttempt { get; set; }
        public PaymentStatus PaymentStates { get; set; }
        public int PaymentID { get; set; }
        [ForeignKey("PaymentID")]
        public virtual ProcessPayment ProcessPayment { get; set; }
    }
}
