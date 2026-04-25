using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.Orders;
using System.Collections.Generic;
using Application.Interfaces;

namespace Application.Features.Order.Query
{
    public class GetRestaurantOrdersQuery : IRequest<Response<List<OrderVM>>>
    {
        public int restaurantId { get; set; }
        public class GetRestaurantOrdersQueryHandler : IRequestHandler<GetRestaurantOrdersQuery, Response<List<OrderVM>>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetRestaurantOrdersQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper,
                                            IAuthenticatedUserService userService)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<List<OrderVM>>> Handle(GetRestaurantOrdersQuery query, CancellationToken cancellationToken)
            {
                var response = await Task.Run(() => _orderRepository.GetOrdersByRestaurantIdAsync(query.restaurantId));
                return new Response<List<OrderVM>>(_mapper.Map<List<OrderVM>>(response), "successful");
            }
        }
    }
}

