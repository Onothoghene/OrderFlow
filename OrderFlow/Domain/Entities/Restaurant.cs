using Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Domain.Entities
{
    public class Restaurant : BaseEntity
    {
        public Restaurant()
        {
            Order = new HashSet<Orders>();
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int? CourierId { get; set; }
        public int OrderId { get; set; }
        public Courier Courier { get; set; }
        public ICollection<Orders> Order { get; set; }

    }
}
