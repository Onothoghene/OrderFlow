using Application.DTOs.CartItem;
using Application.Features.Cart.Command;
using Application.Mappings.Actions;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<AddOrUpdateCartItemCommand, CartItems>()
               .ForMember(dest => dest.Ordered, opt => opt.MapFrom(src => false))
               .ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodId));

            CreateMap<CartItems, CartItemsVM>()
                .ForMember(dest => dest.Food, opt => opt.MapFrom(src => src.Food))
                .AfterMap<CartItemMappingAction>();

        }

    }
}
