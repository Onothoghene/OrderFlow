using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Command
{
    public class UpdateApplicantTypeCommand: IRequest<Response<bool>>
    {
        public int ApplicantType { get; set; }

        public class UpdateApplicantTypeCommandHandler : IRequestHandler<UpdateApplicantTypeCommand, Response<bool>>
        {
            private readonly IUserProfileRepositoryAsync _userProfile;
            private readonly IAuthenticatedUserService _user;

            public UpdateApplicantTypeCommandHandler(IUserProfileRepositoryAsync userProfile, IAuthenticatedUserService user)
            {
                _userProfile = userProfile;
                _user = user;
            }

            public async Task<Response<bool>> Handle(UpdateApplicantTypeCommand request, CancellationToken cancellationToken)
            {
                var applicant = "";

                var userProfile = await _userProfile.GetUserByIdAsync(_user.UserId);

              

                await _userProfile.UpdateAsync(userProfile);

                return new Response<bool>(true, $"applicant now applying as an {applicant}");
            }
        }
    }
}
