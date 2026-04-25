using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.Orders;

namespace Application.Features.Order.Query
{
    public class GetOrderByIdQuery : IRequest<Response<OrderVM>>
    {
        public int orderId { get; set; }

        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Response<OrderVM>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;
            private readonly IMapper _mapper;

            public GetOrderByIdQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
            }
            public async Task<Response<OrderVM>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
            {
                var response = await _orderRepository.GetOrderById(query.orderId);
                if (response == null) throw new ApiException($"The requested order could not be found.");
                return new Response<OrderVM>(_mapper.Map<OrderVM>(response), "successful");
            }
        }
    }
}

