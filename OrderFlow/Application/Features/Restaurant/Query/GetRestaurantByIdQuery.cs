using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.Restaurants;

namespace Application.Features.Restaurant.Query
{
    public class GetRestaurantByIdQuery : IRequest<Response<RestaurantVM>>
    {
        public int restaurantId { get; set; }

        public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, Response<RestaurantVM>>
        {
            private readonly IRestaurantRepositoryAsync _restaurantRepository;
            private readonly IMapper _mapper;

            public GetRestaurantByIdQueryHandler(IRestaurantRepositoryAsync restaurantRepository, IMapper mapper)
            {
                _restaurantRepository = restaurantRepository;
                _mapper = mapper;
            }
            public async Task<Response<RestaurantVM>> Handle(GetRestaurantByIdQuery query, CancellationToken cancellationToken)
            {
                var response = await _restaurantRepository.GetRestaurantbyId(query.restaurantId) 
                                ?? throw new ApiException($"The requested restaurant could not be found.");
                return new Response<RestaurantVM>(_mapper.Map<RestaurantVM>(response), "successful");
            }
        }
    }
}

