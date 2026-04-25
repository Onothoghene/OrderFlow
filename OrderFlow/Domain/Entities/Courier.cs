using Domain.Common;

namespace Domain.Entities
{
    public class Courier : BaseEntity
    {
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
