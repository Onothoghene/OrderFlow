using System.IO.Compression;
using System.IO;
using Application.Interfaces;
using Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using Newtonsoft.Json;
using Application.Helper;

namespace Infrastructure.Shared.Services
{
    public class FileZipService : IFileZipService
    {
        private readonly ZipFileSettings _zipSetting;

        public FileZipService(IOptions<ZipFileSettings> zipSetting)
        {
            _zipSetting = zipSetting.Value;
        }

        public string Zip(string fileFullPathToZip, string uniqueFilename)
        {
            var outputFile = $"{_zipSetting.OutputFilePath}/{uniqueFilename}.zip";

            if (string.IsNullOrEmpty(outputFile))
            {
                return "Output file path not specified.";
            }

            try
            {
                if (!File.Exists(outputFile))
                {
                    using (var archive = ZipFile.Open(outputFile, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(fileFullPathToZip, Path.GetFileName(fileFullPathToZip));
                    }

                    DeleteJsonFile(fileFullPathToZip);

                    return "Zipped successfully";
                }
                else
                {
                    return "The file has already been zippeed";
                }
            }
            catch { return "Zipping failed"; }
        }

        public (string Filepath, string UniqueFilename) WriteOjectToFile(object obj, string fileName)
        {
            string filePath;

            string folderPath = $"{_zipSetting.OutputFilePath}";
            string uniqueFileName = FormatFilename(fileName);
            string newFileType = "json";
            uniqueFileName = uniqueFileName + "." + newFileType;

            filePath = Path.Combine(folderPath, uniqueFileName);

            var _options = new JsonSerializerSettings()
           {
               NullValueHandling = NullValueHandling.Ignore
           };

            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented, _options);

            File.WriteAllText(filePath, jsonString);

            return (filePath, uniqueFileName);
        }

        public bool DeleteJsonFile(string fileFullPath)
        {
            try
            {
                // Check if file exists with its full path
                if (File.Exists(fileFullPath))
                {
                    // If file found, delete it
                    File.Delete(fileFullPath);

                    return true;
                }

                return false;
            }
            catch { return false; }
        }

        private string FormatFilename(string realName) // more parameters tobe added
        {
            if (!string.IsNullOrEmpty(realName))
            {
                if (realName.Contains('.'))
                {
                    realName = Utilities.RemoveExtensionFromFileName(realName);
                }
                return string.Concat(realName, "_", DateTime.UtcNow.Ticks); //concatenate the parameter received form uniquename
            }

            return string.Empty;
        }

     


    }
}
