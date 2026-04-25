using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue("uid"));
            Role = httpContextAccessor.HttpContext?.User?.FindFirstValue("rol");
        }

        public int UserId { get; }
        public string Role { get; }
    }
}
