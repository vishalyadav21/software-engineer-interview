using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zip.Installments.WebApi.Models
{
    public class PaymentPlanDTO
    {
        public Guid Id { get; set; }

        [Required] 
        public decimal PurchaseAmount { get; set; }

        public int PurchaseFrequency { get; set; }

        [Required] 
        public int Installments { get; set; }   
    }
}
