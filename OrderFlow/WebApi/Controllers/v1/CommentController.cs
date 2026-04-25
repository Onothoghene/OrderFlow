using Application.Features.Comment.Command;
using Application.Features.Comment.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CommentController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetCommentsById(int id)
        {
            return Ok(await Mediator.Send(new GetCommentByIdQuery { commentId = id }));
        }

        /// <summary>
        /// Get a menu item comment(s)
        /// </summary>
        /// <param name="menuItemId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("menu-item/{menuItemId?}")]
        public async Task<IActionResult> GetMenuItemComments(int menuItemId)
        {
            return Ok(await Mediator.Send(new GetMenuItemCommentsQuery { menuItemId = menuItemId }));
        }

        /// <summary>
        /// Add or update comments
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdateComments(AddOrUpdateCommentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("menu-item/{userId?}/user")]
        public async Task<IActionResult> GetUserComments(int userId)
        {
            return Ok(await Mediator.Send(new GetUserCommentsQuery { userId = userId }));
        }

        /// <summary>
        /// Get user comment(s)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuItemId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("menu-item/{menuItemId?}/user/{userId?}")]
        public async Task<IActionResult> GetUserComments(int userId, int menuItemId)
        {
            return Ok(await Mediator.Send(new GetUserFoodCommentsQuery { userId = userId, foodId = menuItemId }));
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCommentById(int Id)
        {
            return Ok(await Mediator.Send(new DeleteCommentCommand { commentId = Id }));
        }

    }
}
