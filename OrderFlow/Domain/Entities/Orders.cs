using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Orders : AuditableBaseEntity
    {
        public Orders()
        {
            OrderItems = new HashSet<OrderItems>();
            Payments = new HashSet<Payment>();
        }

        public int RestaurantId { get; set; }
        public int AddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }

        public Address Address { get; set; }
        public Restaurant Restaurant { get; set; }
        public UserProfile CreatedByNavigation { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
