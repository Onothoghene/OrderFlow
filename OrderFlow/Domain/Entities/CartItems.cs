using Domain.Common;

namespace Domain.Entities
{
    public class CartItems : AuditableBaseEntity
    {
        public int Quantity { get; set; }
        public int FoodId { get; set; }
        public bool Ordered { get; set; } = false;
        public MenuItem Food { get; set; }
        public UserProfile CreatedByNavigation { get; set; }
    }

}
