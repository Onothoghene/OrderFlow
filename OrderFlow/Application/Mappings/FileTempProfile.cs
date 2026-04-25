using Application.DTOs.File;
using Application.Mappings.Actions;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class FileTempProfile : Profile
    {
        public FileTempProfile()
        {
            CreateMap<FileTemp, FileVM>()
                .AfterMap<FileTempMappingAction>();

        }

    }
}
