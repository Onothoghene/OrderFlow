using Application.DTOs.File;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.IO;
using System.Linq;

namespace Application.Mappings.Actions
{
    public class FileTempMappingAction : IMappingAction<FileTemp, FileVM>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IFileUploadService _fileUpload;
        string folderPath = Path.GetFullPath("FileUpload/");

        public FileTempMappingAction(IFileUploadService fileUpload, IDateTimeService dateTime)
        {
            _fileUpload = fileUpload;
            _dateTime = dateTime;
        }

        public void Process(FileTemp source, FileVM destination, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.FileUniqueName))
            {
                string filePath = Path.Combine(folderPath, source.FileUniqueName);
                destination.FileBinary = _fileUpload.ConvertFileToBinary(filePath);
            }
            
        }
    }

}
