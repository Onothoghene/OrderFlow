using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Comment.Command
{
    public class DeleteOrderCommand : IRequest<Response<bool>>
    {
        public int orderId { get; set; }

        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Response<bool>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;

            public DeleteOrderCommandHandler(IOrderRepositoryAsync orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<Response<bool>> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = await _orderRepository.GetByIdAsync(command.orderId) ?? 
                                                            throw new ApiException($"The requested order could not be found.");

                    //data.IsDeleted = true;
                    await _orderRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "Order deleted successfully");

                }
            }
        }
    }
}

