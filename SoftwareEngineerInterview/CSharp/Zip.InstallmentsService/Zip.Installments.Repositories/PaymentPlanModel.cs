using System;
using System.Collections.Generic;
using Zip.Installments.DomainEntities;

namespace Zip.Installments.Repositories
{
    public class PaymentPlanModel
    {
        public Guid Id { get; set; }

        public decimal PurchaseAmount { get; set; }

        public int PurchaseFrequency { get; set; }

        public List<Installment> Installments { get; set; }
    }
}