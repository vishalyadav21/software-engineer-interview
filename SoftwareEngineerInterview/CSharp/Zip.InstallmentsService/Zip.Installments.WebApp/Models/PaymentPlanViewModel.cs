using System;

namespace Zip.Installments.WebApp.Models
{
    public class PaymentPlanViewModel
    {
        public Guid Id { get; set; }

        public decimal PurchaseAmount { get; set; }

        public int PurchaseFrequency { get; set; }

        public int Installments { get; set; }   
    }
}
