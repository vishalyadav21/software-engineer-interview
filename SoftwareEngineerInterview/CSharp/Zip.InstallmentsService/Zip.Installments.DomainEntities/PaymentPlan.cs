using System;

namespace Zip.Installments.DomainEntities
{
    /// <summary>
    /// Data structure which defines all the properties for a purchase installment plan.
    /// </summary>
    public class PaymentPlan
    {
        public Guid Id { get; set; }

		public decimal PurchaseAmount { get; set; }

        public int PurchaseFrequency { get; set; }

        public int Installments { get; set; }

    }
}
