using Application.DTOs.PersonalDetails;
using Application.Enums;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.IO;
using System.Linq;

namespace Application.Mappings.Actions
{
    public class PersonalDetailsMappingAction : IMappingAction<UserProfile, PersonalDetailsVM>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IFileUploadService _fileUpload;
        string folderPath = Path.GetFullPath("FileUpload");

        public PersonalDetailsMappingAction(IFileUploadService fileUpload,
            IDateTimeService dateTime)
        {
            //_applicationStages = applicationStages;
            _fileUpload = fileUpload;
            _dateTime = dateTime;
            //_titleRepo = titleRepo;
        }

        public void Process(UserProfile source, PersonalDetailsVM destination, ResolutionContext context)
        {
        }
    }

}
