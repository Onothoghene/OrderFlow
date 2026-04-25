namespace Application.DTOs.MenuItem
{
    public class RatingVM
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
    
    public class RatingIM
    {
        public int MenuItemId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
    }

    public class RatingEM : RatingIM
    {
        public int Id { get; set; }
    }

}
