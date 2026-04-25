using Application.DTOs.MenuItem;

namespace Application.DTOs.CartItem
{
    public class CartItemsVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int FoodId { get; set; }
        public bool Ordered { get; set; }
        public MenuItemVM Food { get; set; }
    }
    
}
