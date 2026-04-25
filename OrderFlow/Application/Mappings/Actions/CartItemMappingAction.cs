using Application.DTOs.CartItem;
using Application.DTOs.File;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings.Actions
{
    public class CartItemMappingAction : IMappingAction<CartItems, CartItemsVM>
    {
        private readonly IMapper _mapper;

        public CartItemMappingAction(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Process(CartItems source, CartItemsVM destination, ResolutionContext context)
        {
            if (source.Food != null && source.Food.Images != null && source.Food.Images.Any())
            {
                destination.Food.Images = _mapper.Map<FileVM>(source.Food.Images.FirstOrDefault());
            }
        }
    }

}
