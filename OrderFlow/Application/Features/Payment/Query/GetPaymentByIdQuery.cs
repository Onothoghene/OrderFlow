using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using Application.DTOs.Payment;

namespace Application.Features.Payment.Query
{
    public class GetPaymentByIdQuery : IRequest<Response<PaymentVM>>
    {
        public int paymentId { get; set; }

        public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Response<PaymentVM>>
        {
            private readonly IPaymentRepositoryAsync _paymentRepository;
            private readonly IMapper _mapper;

            public GetPaymentByIdQueryHandler(IPaymentRepositoryAsync paymentRepository, IMapper mapper)
            {
                _paymentRepository = paymentRepository;
                _mapper = mapper;
            }
            public async Task<Response<PaymentVM>> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
            {
                var response = await _paymentRepository.GetByIdAsync(query.paymentId);
                if (response == null) throw new ApiException($"The requested payment could not be found.");
                return new Response<PaymentVM>(_mapper.Map<PaymentVM>(response), "successful");
            }
        }
    }
}

