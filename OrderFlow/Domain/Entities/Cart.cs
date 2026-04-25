using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Cart : AuditableBaseEntity
    {
        public Cart()
        {
            CartItems = new HashSet<CartItems>();
        }
        public UserProfile CreatedByNavigation { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
    }

}
