using Application.DTOs.MenuItem;
using Application.Enums;
using Application.Features.MenuItem.Command;
using Application.Mappings.Actions;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<AddOrUpdateMenuItemCommand, MenuItem>();

            CreateMap<MenuItem, MenuItemVM>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments != null && src.Comments.Count > 0 ? src.Comments : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images != null && src.Images.Count > 0 ? src.Images.FirstOrDefault() : null))
                .ForMember(dest => dest.AvgRating,
                           opt => opt.MapFrom(src => src.Comments != null && src.Comments.Count > 0
                           ? src.Comments.Average(c => c.Rating) : 0))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => ((FoodCategoryEnum)src.CategoryId).ToString()))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => ((FoodCategoryEnum)src.CategoryId).ToString()))
                .AfterMap<MenuItemMappingAction>();

        }

    }
}
