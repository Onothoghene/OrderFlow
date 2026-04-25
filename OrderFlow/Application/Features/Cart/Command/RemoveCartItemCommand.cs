using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Cart.Command
{
    public class RemoveCartItemCommand : IRequest<Response<bool>>
    {
        public int cartItemId { get; set; }

        public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, Response<bool>>
        {
            private readonly ICartItemRepository _cartItemRepository;

            public RemoveCartItemCommandHandler(ICartItemRepository cartItemRepository)
            {
                _cartItemRepository = cartItemRepository;
            }

            public async Task<Response<bool>> Handle(RemoveCartItemCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = await _cartItemRepository.GetByIdAsync(command.cartItemId) ?? 
                                                            throw new ApiException($"The requested order could not be found.");

                    await _cartItemRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "Item Removed successfully");

                }
            }
        }
    }
}

