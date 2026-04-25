using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Comments : AuditableBaseEntity
    {
        public int FoodId { get; set; }
        public MenuItem Food { get; set; }
        public string CommentText { get; set; }
        public double Rating { get; set; }

        public UserProfile CreatedByNavigation { get; set; }
    }
}
