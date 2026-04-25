using Application.Features.Courier.Command;
using Application.Features.Courier.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourierController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{courierId?}")]
        public async Task<IActionResult> GetCourierById(int courierId)
        {
            return Ok(await Mediator.Send(new GetCourierByIdQuery { courierId = courierId }));
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllCouriers()
        {
            return Ok(await Mediator.Send(new GetAllCouriersQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdateCourier(AddOrUpdateCourierCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCourier(int Id)
        {
            return Ok(await Mediator.Send(new DeleteCourierCommand { courierId = Id }));
        }

    }
}
