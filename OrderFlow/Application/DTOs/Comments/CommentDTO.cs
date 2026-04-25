namespace Application.DTOs.Comments
{
    public class CommentVM
    {
        public int Id { get; set; }
        public int FoodId { get; set; }
        public string CommentText { get; set; }
        public double Rating { get; set; }
    }
    
    public class CommentIM
    {
        public string CommentText { get; set; }
    }

    public class CommentEM : CommentIM
    {
        public int Id { get; set; }
    }

}
