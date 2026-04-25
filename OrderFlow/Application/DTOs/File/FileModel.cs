using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.File
{
    public class FileModel
    {
        //public int Id { get; set; }
        public string FileName { get; set; }
        public string FileFormat { get; set; }
        public string FileBinary { get; set; }
    }
    
    public class FileVM
    {
        //public int Id { get; set; }
        public string ImageURL { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FileUniqueName { get; set; }
        public string FileBinary { get; set; }

    }
}
