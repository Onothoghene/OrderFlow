using Application.DTOs.Settings;
using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Query.GetUserTypes
{
    public class GetUsersByRoleQuery : IRequest<Response<List<GenericSettingsViewModel>>>
    {
        public int RoleId { get; set; }

        public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery,Response<List<GenericSettingsViewModel>>>
        {
            private readonly IUserProfileRepositoryAsync _userProfile;
            private readonly IAccountService _accountService;

            public GetUsersByRoleQueryHandler(IUserProfileRepositoryAsync userProfile, IAccountService accountService)
            {
                _userProfile = userProfile;
                _accountService = accountService;
            }

            public async Task<Response<List<GenericSettingsViewModel>>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
            {
                var roleUsers = new List<GenericSettingsViewModel>();

                var usersId =   _accountService.GetUserIdsByRoleAsync(((Roles)request.RoleId).ToString()).ToList();

                var Users = await Task.Run(() => _userProfile.GetUserProfilesByIds(usersId).ToList());

                foreach (var item in Users)
                {
                    var response = new GenericSettingsViewModel
                    {
                        Name = $"{item.FirstName} {item.LastName}",
                        Id = item.Id,
                    };

                    roleUsers.Add(response);
                }
                return new Response<List<GenericSettingsViewModel>>(roleUsers, "successful");
            }
        }
    }
}
