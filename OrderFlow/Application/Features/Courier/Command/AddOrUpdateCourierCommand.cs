using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;

public class AddOrUpdateCourierCommand : IRequest<Response<bool>>
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public int RestaurantId { get; set; }

    public class AddOrUpdateCourierCommandHandler : IRequestHandler<AddOrUpdateCourierCommand, Response<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ICourierRepositoryAsync _courierRepository;
        private readonly IRestaurantRepositoryAsync _restaurantRepository;

        public AddOrUpdateCourierCommandHandler(IMapper mapper, ICourierRepositoryAsync courierRepository, IRestaurantRepositoryAsync restaurantRepository)
        {
            _mapper = mapper;
            _courierRepository = courierRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Response<bool>> Handle(AddOrUpdateCourierCommand command, CancellationToken cancellationToken)
        {
            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (command.Id.HasValue && command.Id.Value > 0)
                {
                    // Update Existing Courier
                    var courier = await _courierRepository.GetByIdAsync(command.Id.Value);
                    if (courier == null)
                        throw new ApiException($"The requested courier could not be found.");

                    _mapper.Map(command, courier);
                    await _courierRepository.UpdateAsync(courier);

                    // Update the corresponding Restaurant with the updated CourierId
                    var restaurant = await _restaurantRepository.GetByIdAsync(command.RestaurantId);
                    if (restaurant == null)
                        throw new ApiException($"The requested restaurant could not be found.");

                    restaurant.CourierId = courier.Id;
                    await _restaurantRepository.UpdateAsync(restaurant);
                }
                else
                {
                    // Create New Courier
                    var newCourier = _mapper.Map<Courier>(command);
                    await _courierRepository.AddAsync(newCourier);

                    // Update the corresponding Restaurant with the new CourierId
                    var restaurant = await _restaurantRepository.GetByIdAsync(command.RestaurantId);
                    if (restaurant == null)
                        throw new ApiException($"The requested restaurant could not be found.");

                    restaurant.CourierId = newCourier.Id; // Assuming Id is the primary key for Courier
                    await _restaurantRepository.UpdateAsync(restaurant);
                }

                ts.Complete();
        }

            return new Response<bool>(true, "Courier processed successfully.");
        }
    }
}
