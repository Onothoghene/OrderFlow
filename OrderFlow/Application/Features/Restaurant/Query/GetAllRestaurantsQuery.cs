using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.DTOs.Restaurants;
using System.Collections.Generic;

namespace Application.Features.Restaurant.Query
{
    public class GetAllRestaurantsQuery : IRequest<Response<List<RestaurantVM>>>
    {
        public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, Response<List<RestaurantVM>>>
        {
            private readonly IRestaurantRepositoryAsync _restaurantRepository;
            private readonly IMapper _mapper;

            public GetAllRestaurantsQueryHandler(IRestaurantRepositoryAsync restaurantRepository, IMapper mapper)
            {
                _restaurantRepository = restaurantRepository;
                _mapper = mapper;
            }
            public async Task<Response<List<RestaurantVM>>> Handle(GetAllRestaurantsQuery query, CancellationToken cancellationToken)
            {
                var response = await _restaurantRepository.GetAllRestaurants();
                return new Response<List<RestaurantVM>>(_mapper.Map<List<RestaurantVM>>(response), "successful");
            }
        }
    }
}

