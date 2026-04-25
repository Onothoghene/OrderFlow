using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using System.Linq;

namespace Application.Features.Address.Command
{
    public class AddOrUpdateAddressCommand : IRequest<Response<bool>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AdditionalPhoneNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
        public int? Id { get; set; }

        public class AddOrUpdateAddressCommandHandler : IRequestHandler<AddOrUpdateAddressCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _user;
            private readonly IUserProfileRepositoryAsync _userProfile;
            private readonly IAddressRepositoryAsync _addressRepository;

            public AddOrUpdateAddressCommandHandler(IMapper mapper, IAuthenticatedUserService user,
                IUserProfileRepositoryAsync userProfile, IAddressRepositoryAsync addressRepository)
            {
                _mapper = mapper;
                _user = user;
                _userProfile = userProfile;
                _addressRepository = addressRepository;
            }

            public async Task<Response<bool>> Handle(AddOrUpdateAddressCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userId = _user.UserId;

                    //Update functionality
                    if (command.Id.HasValue && command.Id.Value > 0)
                    {
                        var address = await _addressRepository.GetByIdAsync(command.Id.Value);
                        if (address == null)
                            throw new ApiException($"The requested User Address could not be found.");

                        address.City = command.City;
                        address.State = command.State;
                        address.Street = command.Street;
                        address.ZipCode = command.ZipCode;
                        address.Country = command.Country;
                        address.IsDefault = command.IsDefault;

                        // If new address is marked as default, update existing default address
                        if (command.IsDefault)
                        {
                            var currentDefaultAddress = await _addressRepository.GetDefaultUserAddress(userId);
                            if (currentDefaultAddress != null && currentDefaultAddress.Id != address.Id)
                            {
                                currentDefaultAddress.IsDefault = false;
                                await _addressRepository.UpdateAsync(currentDefaultAddress);
                            }
                        }

                        await _addressRepository.UpdateAsync(address);
                    }
                    else //Create Functionality
                    {
                        var data = _mapper.Map<Domain.Entities.Address>(command);

                        // Check if user has any addresses saved
                        //var existingAddresses = await _addressRepository.GetUserAddresses(userId);
                        var existingAddresses = await Task.Run(()=>  _addressRepository.GetUserAddresses(userId));
                        if (existingAddresses != null && existingAddresses.Count() > 0)
                        {
                            // If new address is marked as default, update existing default address
                            var currentDefaultAddress = await _addressRepository.GetDefaultUserAddress(userId);
                            if (currentDefaultAddress != null)
                            {
                                currentDefaultAddress.IsDefault = false;
                                await _addressRepository.UpdateAsync(currentDefaultAddress);
                            }

                        }
                        //else if (command.IsDefault)
                        else    
                        {
                            // If no addresses are saved, set the new address as default
                            data.IsDefault = true;
                        }

                        await _addressRepository.AddAsync(data);
                    }

                    ts.Complete();
                }

                return new Response<bool>(true, "Request excuted successfully.");
            }
        }
    }
}

