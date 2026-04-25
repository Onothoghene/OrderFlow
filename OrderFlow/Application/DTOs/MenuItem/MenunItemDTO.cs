using Application.DTOs.Comments;
using Application.DTOs.File;
using System.Collections.Generic;

namespace Application.DTOs.MenuItem
{
    public class MenuItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public double AvgRating { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public FileVM Images { get; set; }
        public List<CommentVM> Comments { get; set; }
        public bool InUserCart { get; set; } 
        public bool InUserFavorite { get; set; }
    }
    
    public class MenuItemIM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public int CategoryId { get; set; }
    }

    public class MenuItemEM : MenuItemIM
    {
        public int Id { get; set; }
    }

}
