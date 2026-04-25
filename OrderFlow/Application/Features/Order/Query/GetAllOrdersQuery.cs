using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.Orders;
using System.Collections.Generic;

namespace Application.Features.Order.Query
{
    public class GetAllOrdersQuery : IRequest<Response<List<OrderVM>>>
    {
        public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Response<List<OrderVM>>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IMapper _mapper;

            public GetAllOrdersQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
            }
            public async Task<Response<List<OrderVM>>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
            {

                var response = await Task.Run(() => _orderRepository.GetAllOrders());
                return new Response<List<OrderVM>>(_mapper.Map<List<OrderVM>>(response), "successful");
            }
        }
    }
}

