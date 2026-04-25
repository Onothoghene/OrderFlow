using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Payment : AuditableBaseEntity
    {
        public int OrderId { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentOption { get; set; }

        public Orders Order { get; set; }
    }
}
