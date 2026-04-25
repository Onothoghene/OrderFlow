using Application.DTOs.Courier;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CourierProfile : Profile
    {
        public CourierProfile()
        {
            CreateMap<CourierVM, Courier>().ReverseMap()
                .ForMember(dest => dest.HasRestaurant, opt => opt.MapFrom(src => src.Restaurant != null ? true : false))
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.Name : null));

            CreateMap<AddOrUpdateCourierCommand, Courier>();
        }


    }
}
