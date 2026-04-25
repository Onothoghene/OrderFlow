using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PersonalDetails.Command.Update
{
    public class UpdatePersonalDetailsByUserIdCommand : IRequest<Response<int>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public class UpdatePersonalDetailsByUserIdCommandHandler : IRequestHandler<UpdatePersonalDetailsByUserIdCommand, Response<int>>
        {
            private readonly IUserProfileRepositoryAsync _userProfileRepo;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _user;
            private readonly IDateTimeService _dateTime;

            public UpdatePersonalDetailsByUserIdCommandHandler(IUserProfileRepositoryAsync userProfileRepo, 
                                                              IMapper mapper, IAuthenticatedUserService user)
            {
                _userProfileRepo = userProfileRepo;
                _mapper = mapper;
                _user = user;
            }

            public async Task<Response<int>> Handle(UpdatePersonalDetailsByUserIdCommand request, CancellationToken cancellationToken)
            {
                int userId = _user.UserId;

                var userProfile = await _userProfileRepo.GetUserByIdAsync(userId);

                if (userProfile == null) throw new ApiException("User not found.");

                userProfile.FirstName = !string.IsNullOrEmpty(request.FirstName) ? request.FirstName : userProfile.FirstName;
                userProfile.LastName = !string.IsNullOrEmpty(request.LastName) ? request.LastName : userProfile.LastName;

                userProfile.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : userProfile.Email;


                await _userProfileRepo.UpdateAsync(userProfile);



                return new Response<int>(userProfile.Id, "Personal details updated successfully");
            }
        }
    }
}
