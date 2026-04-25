using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class MenuItem : BaseEntity
    {
        public MenuItem()
        {
            //Ratings = new HashSet<MenuItemRating>();
            Images = new HashSet<FileTemp>();
            Comments = new HashSet<Comments>();
            CartItems = new HashSet<CartItems>();
        }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public int StockQuantity { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //public ICollection<MenuItemRating> Ratings { get; set; }
        public ICollection<FileTemp> Images { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
    }
}
