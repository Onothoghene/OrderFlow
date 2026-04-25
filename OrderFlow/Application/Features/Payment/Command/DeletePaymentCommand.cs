using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Payment.Command
{
    public class DeletePaymentCommand : IRequest<Response<bool>>
    {
        public int paymentId { get; set; }

        public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, Response<bool>>
        {
            private readonly IPaymentRepositoryAsync _paymentRepository;

            public DeletePaymentCommandHandler(IPaymentRepositoryAsync paymentRepository)
            {
                _paymentRepository = paymentRepository;
            }

            public async Task<Response<bool>> Handle(DeletePaymentCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var payment = await _paymentRepository.GetByIdAsync(command.paymentId) ??
                        throw new ApiException($"The requested comment could not be found.");

                    await _paymentRepository.DeleteAsync(payment);

                    ts.Complete();
                    return new Response<bool>(true, "Food Comment deleted successfully");

                }
            }
        }
    }
}

