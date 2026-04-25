using Domain.Common;

namespace Domain.Entities
{
    public class FileTemp : BaseEntity
    {
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FileUniqueName { get; set; }
        public string ImageURL { get; set; }

        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
