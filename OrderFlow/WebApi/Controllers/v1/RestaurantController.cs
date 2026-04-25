using Application.Features.Restaurant.Command;
using Application.Features.Restaurant.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RestaurantController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{restaurantId?}")]
        public async Task<IActionResult> GetRestaurantById(int restaurantId)
        {
            return Ok(await Mediator.Send(new GetRestaurantByIdQuery { restaurantId = restaurantId }));
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            return Ok(await Mediator.Send(new GetAllRestaurantsQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdateRestaurant(AddOrUpdateRestaurantCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteRestaurant(int Id)
        {
            return Ok(await Mediator.Send(new DeleteRestaurantCommand { restaurantId = Id }));
        }

    }
}
