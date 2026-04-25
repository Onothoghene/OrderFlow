using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.DTOs.Address;
using Application.Interfaces.Repositories;

namespace Application.Features.Address.Query
{
    public class GetAddressByIdQuery : IRequest<Response<AddressVM>>
    {
        public int addressId { get; set; }

        public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Response<AddressVM>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            private readonly IMapper _mapper;

            public GetAddressByIdQueryHandler(IAddressRepositoryAsync addressRepository, IMapper mapper)
            {
                _addressRepository = addressRepository;
                _mapper = mapper;
            }
            public async Task<Response<AddressVM>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
            {
                var response = await _addressRepository.GetByIdAsync(query.addressId);
                if (response == null) throw new ApiException($"The requested address could not be found.");
                return new Response<AddressVM>(_mapper.Map<AddressVM>(response), "successful");
            }
        }
    }
}

