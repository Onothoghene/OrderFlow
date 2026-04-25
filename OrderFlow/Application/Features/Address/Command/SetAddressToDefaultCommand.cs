using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;

namespace Application.Features.Address.Command
{
    public class SetAddressToDefaultCommand : IRequest<Response<bool>>
    {
        public int addressId { get; set; }

        public class SetAddressToDefaultCommandHandler : IRequestHandler<SetAddressToDefaultCommand, Response<bool>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            private readonly IAuthenticatedUserService _userService;

            public SetAddressToDefaultCommandHandler(IAddressRepositoryAsync addressRepository,
                                                    IAuthenticatedUserService userService)
            {
                _addressRepository = addressRepository;
                _userService = userService;
            }

            public async Task<Response<bool>> Handle(SetAddressToDefaultCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userId = _userService.UserId;
                    if (command.addressId > 0)
                    {
                        var address = await _addressRepository.GetByIdAsync(command.addressId);
                        if (address == null)
                            throw new ApiException($"The requested User Address could not be found.");

                        var currentDefaultAddress = await _addressRepository.GetDefaultUserAddress(userId);

                        if (currentDefaultAddress != null)
                        {
                            currentDefaultAddress.IsDefault = false;

                            await _addressRepository.UpdateAsync(currentDefaultAddress);
                        }

                        address.IsDefault = true;

                        await _addressRepository.UpdateAsync(address);
                    }

                    ts.Complete();
                }

                return new Response<bool>(true, "Request excuted successfully.");
            }
        }
    }
}

