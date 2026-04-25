using Application.Features.Comment.Command;
using Application.Features.Payment.Command;
using Application.Features.Payment.Query;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentController : BaseApiController
    {
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            return Ok(await Mediator.Send(new GetPaymentByIdQuery { paymentId = id }));
        }

        [HttpPut("")]
        public async Task<IActionResult> AddOrUpdatePayment(AddOrUpdatePaymentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePaymentById(int Id)
        {
            return Ok(await Mediator.Send(new DeletePaymentCommand { paymentId = Id }));
        }

    }
}
