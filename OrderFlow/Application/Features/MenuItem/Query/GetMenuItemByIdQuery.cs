using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.MenuItem;

namespace Application.Features.MenuItem.Query
{
    public class GetMenuItemByIdQuery : IRequest<Response<MenuItemVM>>
    {
        public int menuItemId { get; set; }

        public class GetMenuItemByIdQueryHandler : IRequestHandler<GetMenuItemByIdQuery, Response<MenuItemVM>>
        {
            private readonly IMenuItemRepositoryAsync _menuItemRepository;
            private readonly IMapper _mapper;

            public GetMenuItemByIdQueryHandler(IMenuItemRepositoryAsync menuItemRepository, IMapper mapper)
            {
                _menuItemRepository = menuItemRepository;
                _mapper = mapper;
            }
            public async Task<Response<MenuItemVM>> Handle(GetMenuItemByIdQuery query, CancellationToken cancellationToken)
            {
                var response = await _menuItemRepository.GetMenuItemById(query.menuItemId) 
                                        ?? throw new ApiException($"The requested Menu Item could not be found.");

                return new Response<MenuItemVM>(_mapper.Map<MenuItemVM>(response), "successful");
            }
        }
    }
}

