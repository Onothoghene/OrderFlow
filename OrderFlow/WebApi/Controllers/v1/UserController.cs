using Application.Features.Users.Command;
using Application.Features.Users.Query.GetUsersQuery;
using Application.Features.Users.Query.GetUserTypes;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class UserController : BaseApiController
    {
        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetUsersByRole(int roleId)
        {
            return Ok(await Mediator.Send(new GetUsersByRoleQuery { RoleId = roleId }));
        }


        //[HttpPut("applicant-type")]
        //public async Task<IActionResult> UpdateApplicantType(UpdateApplicantTypeCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}

        [HttpGet("data")]
        public async Task<IActionResult> GetUsers(bool IsAdvocate)
        {
            return Ok(await Mediator.Send(new GetUsersQuery { IsAdvocate = IsAdvocate }));
        }

    }
}
