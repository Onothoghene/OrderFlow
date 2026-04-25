using Application.DTOs.Orders;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItemsIM, OrderItems>();
            CreateMap<OrderItems, OrderItemsVM>()
                .ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodId));
        }

    }
}
