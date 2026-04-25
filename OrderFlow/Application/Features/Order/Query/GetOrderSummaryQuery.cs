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
    public class GetOrderSummaryQuery : IRequest<Response<OrderSummaryVM>>
    {
        public int orderId { get; set; }
        public class GetOrderSummaryQueryHandler : IRequestHandler<GetOrderSummaryQuery, Response<OrderSummaryVM>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetOrderSummaryQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper,
                                            IAuthenticatedUserService userService)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<OrderSummaryVM>> Handle(GetOrderSummaryQuery query, CancellationToken cancellationToken)
            {
                var response = await Task.Run(() => _orderRepository.GetOrderSummary(query.orderId));
                return new Response<OrderSummaryVM>(_mapper.Map<OrderSummaryVM> (response), "successful");
            }
        }
    }
}

