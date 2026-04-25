using Application.DTOs.Account;
using Application.Wrappers;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<string>> RegisterAsync(RegisterRequest request);
        Task<Response<bool>> VerifyUser(int otp);
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, bool isOffline = false);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task<Response<bool>> ForgotPassword(ForgotPasswordRequest model);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> ChangePassword(ChangePasswordRequest model);
        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token);
        List<int> GetUserIdsByRoleAsync(string role);
        Task<Response<string>> ResendVerificationMail(string email);

        Task<string> GetUserRoleByEmail(string email);
        Task<string> GetUserRoleById(int userId);
        Task<Response<bool>> VerifyOtp(int otp);
        Response<AuthenticationResponse> PeriodicAuthentication(AuthenticationRequest request);
        Task<Response<string>> CreateAdmin(RegisterRequest request);
        List<int> GetUsersAsync();
        List<int> GetAdminsAsync();
    }
}
