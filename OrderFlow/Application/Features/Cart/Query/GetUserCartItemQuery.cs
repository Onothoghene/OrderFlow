using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using System.Collections.Generic;
using Application.DTOs.CartItem;
using Application.Interfaces;

namespace Application.Features.Cart.Query
{
    public class GetUserCartItemQuery : IRequest<Response<List<CartItemsVM>>>
    {
        public class GetUserCartItemQueryHandler : IRequestHandler<GetUserCartItemQuery, Response<List<CartItemsVM>>>
        {
            private readonly ICartItemRepository _cartItemRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetUserCartItemQueryHandler(ICartItemRepository cartItemRepository, IMapper mapper,
                                               IAuthenticatedUserService userService)
            {
                _cartItemRepository = cartItemRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<List<CartItemsVM>>> Handle(GetUserCartItemQuery query, CancellationToken cancellationToken)
            {
                var response = await Task.Run(() => _cartItemRepository.GetCartByUserIdAsync(_userService.UserId));
                return new Response<List<CartItemsVM>>(_mapper.Map<List<CartItemsVM>>(response), "successful");
            }
        }
    }
}

