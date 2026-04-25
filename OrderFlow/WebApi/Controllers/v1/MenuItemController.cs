using Application.Features.MenuItem.Command;
using Application.Features.MenuItem.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MenuItemController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            return Ok(await Mediator.Send(new GetMenuItemByIdQuery { menuItemId = id }));
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetMenuItems()
        {
            return Ok(await Mediator.Send(new GetMenuItemsQuery() ));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdateMenuItem(AddOrUpdateMenuItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMenuItem(int Id)
        {
            return Ok(await Mediator.Send(new DeleteMenuItemCommand { menuItemId = Id }));
        }

    }
}
