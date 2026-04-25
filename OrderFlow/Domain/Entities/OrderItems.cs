using Domain.Common;

namespace Domain.Entities
{
    public class OrderItems : BaseEntity
    {
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        public Orders Orders { get; set; }
        public MenuItem Food { get; set; }
    }
}
