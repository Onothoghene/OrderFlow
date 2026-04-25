using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpires { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public bool HasPreference { get; set; }
    }
}
