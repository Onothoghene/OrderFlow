using Application.DTOs.Orders;
using Application.Features.Order.Command;
using Application.Mappings.Actions;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrUpdateOrderCommand, Orders>()
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Orders, OrderVM>()
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems != null && src.OrderItems.Count() > 0 ? src.OrderItems : null));

            CreateMap<Orders, OrderSummaryVM>()
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems != null && src.OrderItems.Count() > 0 ? src.OrderItems : null))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? null))
               .ForMember(dest => dest.Restaurant, opt => opt.MapFrom(src => src.Restaurant ?? null))
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.CreatedByNavigation ?? null))
               //.ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payments.Where() ?? null))
               .AfterMap<OrderSummaryMappingAction>();
        }

    }
}
