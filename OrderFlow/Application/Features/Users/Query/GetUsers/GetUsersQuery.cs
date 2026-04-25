using Application.DTOs.PersonalDetails;
using Application.DTOs.Settings;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Query.GetUsersQuery
{
    public class GetUsersQuery : IRequest<Response<bool>>
    {
        public bool IsAdvocate { get; set; }

        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery,Response<bool>>
        {
            private readonly IUserProfileRepositoryAsync _userProfile;
            private readonly IAccountService _accountService;
            private readonly IMapper _mapper;

            public GetUsersQueryHandler(IUserProfileRepositoryAsync userProfile, IAccountService accountService, IMapper mapper)
            {
                _userProfile = userProfile;
                _accountService = accountService;
                _mapper = mapper;
            }

            public async Task<Response<bool>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
              
                return new Response<bool>(true, "successful");
            }
        }
    }
}
