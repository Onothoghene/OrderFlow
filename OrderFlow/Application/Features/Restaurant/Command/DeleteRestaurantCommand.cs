using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Restaurant.Command
{
    public class DeleteRestaurantCommand : IRequest<Response<bool>>
    {
        public int restaurantId { get; set; }

        public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Response<bool>>
        {
            private readonly IRestaurantRepositoryAsync _restaurantRepository;

            public DeleteRestaurantCommandHandler(IRestaurantRepositoryAsync restaurantRepository)
            {
                _restaurantRepository = restaurantRepository;
            }

            public async Task<Response<bool>> Handle(DeleteRestaurantCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = await _restaurantRepository.GetByIdAsync(command.restaurantId) ?? 
                                                            throw new ApiException($"The requested restaurant could not be found.");

                    //data.IsDeleted = true;
                    await _restaurantRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "Restaurant deleted successfully");

                }
            }
        }
    }
}

