using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zip.Installments.WebApp.Models
{
    public class PaymentPlanViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Purchase Amount")]
        [RegularExpression(@"^(0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*)$")]
        public decimal PurchaseAmount { get; set; }

        public int PurchaseFrequency { get; set; }

        [Required]
        [RegularExpression(@"^[1-9][0-9]*$")]
        public int Installments { get; set; }   
    }
}
