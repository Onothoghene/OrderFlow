using Application.DTOs.PersonalDetails;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PersonalDetails.Query.GetById
{
    public class GetPersonalDetailsByIdLiteQuery : IRequest<Response<PersonalDetailsVM>>
    {
        public int? Id { get; set; }

        public class GetPersonalDetailsByIdLiteQueryHandler : IRequestHandler<GetPersonalDetailsByIdLiteQuery, Response<PersonalDetailsVM>>
        {
            private readonly IUserProfileRepositoryAsync _userProfile;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _user;

            public GetPersonalDetailsByIdLiteQueryHandler(IUserProfileRepositoryAsync userProfile, IMapper mapper, IAuthenticatedUserService user)
            {
                _userProfile = userProfile;
                _mapper = mapper;
                _user = user;
            }

            public async Task<Response<PersonalDetailsVM>>Handle(GetPersonalDetailsByIdLiteQuery request, CancellationToken cancellationToken)
            {
                var userId = request.Id.HasValue && request.Id.Value > 0 ? request.Id.Value : _user.UserId;

                var personalDetails = await _userProfile.GetUserByIdAsync(userId);

                if (personalDetails == null) throw new ApiException("User personal details not found");

                var resp = _mapper.Map<PersonalDetailsVM>(personalDetails);

                return new Response<PersonalDetailsVM>(resp, "successful");
            }
        }
    }
}
