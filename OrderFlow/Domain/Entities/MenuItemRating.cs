using Domain.Common;

namespace Domain.Entities
{
    public class MenuItemRating : AuditableBaseEntity
    {
        public int MenuItemId { get; set; }
        public int Rating { get; set; } // Scale of 1-5
        public string Review { get; set; }

        public MenuItem MenuItem { get; set; }
        public UserProfile CreatedByNavigation { get; set; }
    }
}
