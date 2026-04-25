using Application.Features.PersonalDetails.Command.Update;
using Application.Features.PersonalDetails.Query.GetById;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonalDetailsController : BaseApiController
    {
        [HttpGet("user/{id?}")]
        public async Task<IActionResult> GetPersonalDetailsByUserId(int id)
        {
            return Ok(await Mediator.Send(new GetPersonalDetailsByIdQuery { Id = id }));
        }

        [HttpGet("user/lite/{id?}")]
        public async Task<IActionResult> GetPersonalDetailsByUserIdLite(int id)
        {
            return Ok(await Mediator.Send(new GetPersonalDetailsByIdLiteQuery { Id = id }));
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdatePersonalDetailsByUserId(UpdatePersonalDetailsByUserIdCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}
