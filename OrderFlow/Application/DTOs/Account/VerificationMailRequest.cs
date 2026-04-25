using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Account
{
    public class VerificationMailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
