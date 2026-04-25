using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Address.Command
{
    public class DeleteAddressCommand : IRequest<Response<bool>>
    {
        public int addressId { get; set; }

        public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Response<bool>>
        {
            private readonly IAuthenticatedUserService _user;
            private readonly IUserProfileRepositoryAsync _userProfile;
            private readonly IAddressRepositoryAsync _addressRepository;

            public DeleteAddressCommandHandler(IMapper mapper, IAuthenticatedUserService user,
                IUserProfileRepositoryAsync userProfile, IAddressRepositoryAsync addressRepository)
            {
                _user = user;
                _userProfile = userProfile;
                _addressRepository = addressRepository;
            }

            public async Task<Response<bool>> Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = await _addressRepository.GetByIdAsync(command.addressId) ?? throw new ApiException($"The requested address could not be found.");

                    //data.IsDeleted = true;
                    await _addressRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "User Address deleted successfully");

                }
            }
        }
    }
}

