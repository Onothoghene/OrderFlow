using Application.DTOs.Restaurants;
using Application.Features.Restaurant.Command;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantVM>()
                .ForMember(dest => dest.HasCourier, opt => opt.MapFrom(src => src.Courier != null ? true : false))
                .ForMember(dest => dest.Courier, opt => opt.MapFrom(src => src.Courier != null ? src.Courier : null));

            CreateMap<AddOrUpdateRestaurantCommand, Restaurant>();
        }


    }
}
