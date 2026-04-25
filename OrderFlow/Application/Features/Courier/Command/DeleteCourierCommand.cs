using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Courier.Command
{
    public class DeleteCourierCommand : IRequest<Response<bool>>
    {
        public int courierId { get; set; }

        public class DeleteCourierCommandHandler : IRequestHandler<DeleteCourierCommand, Response<bool>>
        {
            private readonly ICourierRepositoryAsync _courierRepository;

            public DeleteCourierCommandHandler(ICourierRepositoryAsync courierRepository)
            {
                _courierRepository = courierRepository;
            }

            public async Task<Response<bool>> Handle(DeleteCourierCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = await _courierRepository.GetByIdAsync(command.courierId) ??
                                                            throw new ApiException($"The requested courier could not be found.");

                    //data.IsDeleted = true;
                    await _courierRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "Courier deleted successfully");

                }
            }
        }
    }
}

