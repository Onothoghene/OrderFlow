using Application.DTOs.Account;
using Application.Features.ContactUs;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApplicationSupportController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public ApplicationSupportController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("resend-verification-mail")]
        public async Task<IActionResult> ResendConfirmationMail(VerificationMailRequest request)
        {
            return Ok(await _accountService.ResendVerificationMail(request.Email));
        }

        [HttpPost("contact-us-mail")]
        [AllowAnonymous]
        public async Task<IActionResult> ContactUsMail(ContactUsMail command)
        {
            return Ok(await Mediator.Send(command));
        }

    }

}
