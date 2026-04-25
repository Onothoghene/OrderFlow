using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.DTOs.Courier;

namespace Application.Features.Restaurant.Command
{
    public class AddOrUpdateRestaurantCommand : IRequest<Response<bool>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; } = true;
        //public int CourierId { get; set; }

        public class AddOrUpdateRestaurantCommandHandler : IRequestHandler<AddOrUpdateRestaurantCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IRestaurantRepositoryAsync _restaurantRepository;

            public AddOrUpdateRestaurantCommandHandler(IMapper mapper, IRestaurantRepositoryAsync restaurantRepository)
            {
                _mapper = mapper;
                _restaurantRepository = restaurantRepository;
            }

            public async Task<Response<bool>> Handle(AddOrUpdateRestaurantCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Update functionality
                    if (command.Id.HasValue && command.Id.Value > 0)
                    {
                        var response = await _restaurantRepository.GetByIdAsync(command.Id.Value);
                        if (response == null)
                            throw new ApiException($"The requested restaurant could not be found.");

                        response.Name = command.Name;
                        response.Location = command.Location;
                        //response.CourierId = command.CourierId;

                        await _restaurantRepository.UpdateAsync(response);
                    }
                    else //Create Functionality
                    {
                        var data = _mapper.Map<Domain.Entities.Restaurant>(command);

                        await _restaurantRepository.AddAsync(data);
                    }

                    ts.Complete();
                }

                return new Response<bool>(true, "Command excuted successfully.");
            }
        }
    }
}

