using Application.DTOs.MenuItem;

namespace Application.DTOs.Orders
{
    public class OrderItemsVM
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public MenuItemVM Food { get; set; }

    }
    
    public class OrderItemsIM
    {
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class OrderItemsEM : OrderItemsIM
    {
        public int Id { get; set; }
    }

}
