using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using System.Collections.Generic;
using Application.DTOs.Courier;

namespace Application.Features.Courier.Query
{
    public class GetAllCouriersQuery : IRequest<Response<List<CourierVM>>>
    {
        public class GetAllCouriersQueryHandler : IRequestHandler<GetAllCouriersQuery, Response<List<CourierVM>>>
        {
            private readonly ICourierRepositoryAsync _courierRepository;
            private readonly IMapper _mapper;

            public GetAllCouriersQueryHandler(ICourierRepositoryAsync courierRepository, IMapper mapper)
            {
                _courierRepository = courierRepository;
                _mapper = mapper;
            }
            public async Task<Response<List<CourierVM>>> Handle(GetAllCouriersQuery query, CancellationToken cancellationToken)
            {
                var response = await Task.Run(() => _courierRepository.GetAllCouriers());
                return new Response<List<CourierVM>>(_mapper.Map<List<CourierVM>>(response), "successful");
            }
        }
    }
}

