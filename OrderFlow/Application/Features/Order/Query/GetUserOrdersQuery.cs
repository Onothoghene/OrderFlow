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
    public class GetUserOrdersQuery : IRequest<Response<List<OrderVM>>>
    {
        public int? userId { get; set; }
        public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, Response<List<OrderVM>>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetUserOrdersQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper,
                                            IAuthenticatedUserService userService)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<List<OrderVM>>> Handle(GetUserOrdersQuery query, CancellationToken cancellationToken)
            {
                var user = query.userId.HasValue && query.userId > 0 ? query.userId.Value : _userService.UserId;
                var response = await Task.Run(() => _orderRepository.GetUserOrders(user));
                return new Response<List<OrderVM>>(_mapper.Map<List<OrderVM>>(response), "successful");
            }
        }
    }
}

