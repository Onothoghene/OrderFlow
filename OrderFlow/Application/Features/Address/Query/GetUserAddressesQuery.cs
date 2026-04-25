using Application.Exceptions;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.DTOs.Address;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using System.Collections.Generic;

namespace Application.Features.Address.Query
{
    public class GetUserAddressesQuery : IRequest<Response<List<AddressVM>>>
    {
        public int? userId { get; set; }

        public class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, Response<List<AddressVM>>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetUserAddressesQueryHandler(IAddressRepositoryAsync addressRepository, IMapper mapper,
                                                IAuthenticatedUserService userService)
            {
                _addressRepository = addressRepository;
                _userService = userService;
                _mapper = mapper;
            }
            public async Task<Response<List<AddressVM>>> Handle(GetUserAddressesQuery query, CancellationToken cancellationToken)
            {
                var user = query.userId.HasValue && query.userId.Value > 0 ? query.userId.Value : _userService.UserId;
                var response = await Task.Run(()=>_addressRepository.GetUserAddresses(user));
                //if (response == null) throw new ApiException($"The requested project could not be found.");
                return new Response<List<AddressVM>>(_mapper.Map<List<AddressVM>>(response), "successful");
            }
        }
    }
}

