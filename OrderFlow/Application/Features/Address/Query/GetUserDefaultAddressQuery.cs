using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.DTOs.Address;
using Application.Interfaces.Repositories;
using Application.Interfaces;

namespace Application.Features.Address.Query
{
    public class GetUserDefaultAddressQuery : IRequest<Response<AddressVM>>
    {
        public int? userId { get; set; }

        public class GetUserDefaultAddressQueryHandler : IRequestHandler<GetUserDefaultAddressQuery, Response<AddressVM>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetUserDefaultAddressQueryHandler(IAddressRepositoryAsync addressRepository, IMapper mapper,
                                                    IAuthenticatedUserService userService)
            {
                _addressRepository = addressRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<AddressVM>> Handle(GetUserDefaultAddressQuery query, CancellationToken cancellationToken)
            {
                var user = query.userId.HasValue && query.userId.Value > 0 ? query.userId.Value : _userService.UserId;
                var response = await _addressRepository.GetDefaultUserAddress(user);
                //if (response == null) throw new ApiException($"The requested address could not be found.");
                return new Response<AddressVM>(_mapper.Map<AddressVM>(response), "successful");
            }
        }
    }
}

