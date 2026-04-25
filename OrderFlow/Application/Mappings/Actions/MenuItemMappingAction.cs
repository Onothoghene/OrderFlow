using Application.DTOs.MenuItem;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings.Actions
{
    public class MenuItemMappingAction : IMappingAction<MenuItem, MenuItemVM>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _userService;

        public MenuItemMappingAction(IMapper mapper, IAuthenticatedUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public void Process(MenuItem source, MenuItemVM destination, ResolutionContext context)
        {
            if (source.CartItems != null && source.CartItems.Count() > 0)
            {
                destination.InUserCart = source.CartItems.Any(r => !r.Ordered && r.CreatedBy == _userService.UserId);
            }
        }
    }

}
