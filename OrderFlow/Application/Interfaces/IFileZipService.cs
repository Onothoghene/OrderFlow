using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IFileZipService
    {
        string Zip(string fileToZip, string table);
        (string Filepath, string UniqueFilename) WriteOjectToFile(object obj, string fileName);
        bool DeleteJsonFile(string fileFullPath);
    }
}
