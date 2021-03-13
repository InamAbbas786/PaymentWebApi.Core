using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DBHepler.Core.Entities
{
    public class PaymentLogs
    {
        public PaymentLogs()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        public string Desc { get; set; }
        public string Action { get; set; }
        public string PaymentMethod { get; set; }
        public int ActionID { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
