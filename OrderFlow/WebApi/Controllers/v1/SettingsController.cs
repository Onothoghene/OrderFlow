using Application.DTOs.Settings;
using Application.Features.Resoures.Query;
using Application.Features.Settings.Command;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SettingsController : BaseApiController
    {
       

        [HttpGet("resource-links")]
        public async Task<IActionResult> GetResourceLinks()
        {
            return Ok(await Mediator.Send(new GetResourceLinksQuery()));
        }



    }
}
