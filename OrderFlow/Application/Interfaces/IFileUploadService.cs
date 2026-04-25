using Application.DTOs.File;
using Domain.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileUploadService
    {
        string ConvertToFile(string base64String, string folderPath, string realFileName);
        string ConvertToFileAndReplace(string base64String, string folderPath, string uniqueFileName);
        string ConvertFileToBinary(string fileFullPath);
        Task<string> GetImageAsBase64Url(string url);
        string CalculateFileSize(string base64string);
        string SaveBase64File(string base64String, string fileName);
        void DeleteFile(string uniqueFileName);
    }
}
