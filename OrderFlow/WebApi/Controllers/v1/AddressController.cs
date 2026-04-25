using Application.Features.Address.Command;
using Application.Features.Address.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AddressController : BaseApiController
    {
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            return Ok(await Mediator.Send(new GetAddressByIdQuery { addressId = id }));
        }

        [HttpGet("user/{userId?}")]
        public async Task<IActionResult> GetUserAddresses(int userId)
        {
            return Ok(await Mediator.Send(new GetUserAddressesQuery { userId = userId }));
        }

        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdateAddress(AddOrUpdateAddressCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAddressById(int Id)
        {
            return Ok(await Mediator.Send(new DeleteAddressCommand { addressId = Id }));
        }

        [HttpPut("default")]
        public async Task<IActionResult> SetAddressToDefault(SetAddressToDefaultCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("default/user/{userId?}")]
        public async Task<IActionResult> GetUserDefaultAddress(int userId)
        {
            return Ok(await Mediator.Send(new GetUserDefaultAddressQuery { userId = userId }));
        }

    }
}
