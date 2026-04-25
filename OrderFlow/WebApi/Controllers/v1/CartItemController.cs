using Application.Features.Cart.Command;
using Application.Features.Cart.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartItemController : BaseApiController
    {
        /// <summary>
        /// Used to get user cart item list
        /// </summary>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<IActionResult> GetMenuItemComments()
        {
            return Ok(await Mediator.Send(new GetUserCartItemQuery()));
        }

        /// <summary>
        /// Used to add or update user cart
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdateCartItem(AddOrUpdateCartItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Used to remove an item from a cart
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> RemoveCartItem(int Id)
        {
            return Ok(await Mediator.Send(new RemoveCartItemCommand { cartItemId = Id }));
        }

    }
}
