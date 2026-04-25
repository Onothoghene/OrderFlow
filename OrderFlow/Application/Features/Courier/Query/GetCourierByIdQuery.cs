using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.Restaurants;
using Application.DTOs.Courier;

namespace Application.Features.Courier.Query
{
    public class GetCourierByIdQuery : IRequest<Response<CourierVM>>
    {
        public int courierId { get; set; }

        public class GetCourierByIdQueryHandler : IRequestHandler<GetCourierByIdQuery, Response<CourierVM>>
        {
            private readonly ICourierRepositoryAsync _courierRepository;
            private readonly IMapper _mapper;

            public GetCourierByIdQueryHandler(ICourierRepositoryAsync courierRepository, IMapper mapper)
            {
                _courierRepository = courierRepository;
                _mapper = mapper;
            }
            public async Task<Response<CourierVM>> Handle(GetCourierByIdQuery query, CancellationToken cancellationToken)
            {
                var response = await _courierRepository.GetByIdAsync(query.courierId) 
                                ?? throw new ApiException($"The requested courier could not be found.");
                return new Response<CourierVM>(_mapper.Map<CourierVM>(response), "successful");
            }
        }
    }
}

