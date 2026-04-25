using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;

namespace Application.Features.Comment.Command
{
    public class AddOrUpdatePaymentCommand : IRequest<Response<bool>>
    {
        public int? Id { get; set; }
        public int OrderId { get; set; }
        public decimal AmountPaid { get; set; }

        public class AddOrUpdatePaymentCommandHandler : IRequestHandler<AddOrUpdatePaymentCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _user;
            private readonly IPaymentRepositoryAsync _paymentRepository;

            public AddOrUpdatePaymentCommandHandler(IMapper mapper, IAuthenticatedUserService user,
                                                   IPaymentRepositoryAsync paymentRepository)
            {
                _mapper = mapper;
                _user = user;
                _paymentRepository = paymentRepository;
            }

            public async Task<Response<bool>> Handle(AddOrUpdatePaymentCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userId = _user.UserId;

                    //Update functionality
                    if (command.Id.HasValue && command.Id.Value > 0)
                    {
                        var payment = await _paymentRepository.GetByIdAsync(command.Id.Value);
                        if (payment == null)
                            throw new ApiException($"The requested payment could not be found.");

                        payment.AmountPaid = command.AmountPaid;
                        payment.OrderId = command.OrderId;

                        await _paymentRepository.UpdateAsync(payment);
                    }
                    else //Create Functionality
                    {
                        var data = _mapper.Map<Domain.Entities.Payment>(command);

                        await _paymentRepository.AddAsync(data);
                    }

                    ts.Complete();
                }

                return new Response<bool>(true, "Request excuted successfully.");
            }
        }
    }
}

