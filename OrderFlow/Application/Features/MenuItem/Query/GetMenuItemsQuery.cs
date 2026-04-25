using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.DTOs.MenuItem;
using System.Collections.Generic;
using System.Linq;

namespace Application.Features.MenuItem.Query
{
    public class GetMenuItemsQuery : IRequest<Response<List<MenuItemVM>>>
    {
        public class GetMenuItemsQueryHandler : IRequestHandler<GetMenuItemsQuery, Response<List<MenuItemVM>>>
        {
            private readonly IMenuItemRepositoryAsync _menuItemRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetMenuItemsQueryHandler(IMenuItemRepositoryAsync menuItemRepository, IMapper mapper,
                                            IAuthenticatedUserService userService)
            {
                _menuItemRepository = menuItemRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<List<MenuItemVM>>> Handle(GetMenuItemsQuery query, CancellationToken cancellationToken)
            {
                var userId = _userService.UserId;
                var menuItems = await Task.Run(()=> _menuItemRepository.GetAllMenuItems());

                var data = _mapper.Map<List<MenuItemVM>>(menuItems);

                // Set InUserCart manually
                //foreach (var item in data)
                //{
                //    var menuItem = menuItems.FirstOrDefault(m => m.Id == item.Id);
                //    if (menuItem != null)
                //    {
                //        item.InUserCart = menuItem.CartItems.Any(c => c.CreatedBy == userId && c.Ordered == false);
                //    }
                //}

                return new Response<List<MenuItemVM>>(data, "successful");
            }
        }
    }
}

