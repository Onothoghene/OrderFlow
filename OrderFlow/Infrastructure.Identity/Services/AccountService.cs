using Application.DTOs.Account;
using Application.DTOs.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Settings;
using Infrastructure.Identity.Context;
using Infrastructure.Identity.Helpers;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IdentityContext _context;
        private readonly IUserProfileRepositoryAsync _userProfile;
        private readonly ApplicationUrl _applicationUrlSettings;

        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IdentityContext context,
            IUserProfileRepositoryAsync userProfile,
             IOptionsSnapshot<ApplicationUrl> applicationUrlSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            this._emailService = emailService;
            _context = context;
            _userProfile = userProfile;
            _applicationUrlSettings = applicationUrlSettings.Value;
        }

        //Registration Method
        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.Email);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Email '{request.Email}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var otp = GenerateOTP();

                    await _userManager.AddToRoleAsync(user, ((Roles)request.RoleId).ToString());

                    //use the request to create a new user profile
                    var userProfile = new UserProfile
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        VerificationCode = otp
                    };

                    var resp = await _userProfile.AddAsync(userProfile);

                    if (resp.Id > 0)
                    {

                        var templatePath = "EmailTemplate/ConfirmEmail.cshtml";

                        await _emailService.SendFluentEmailTemplate(new EmailRequest()
                        {
                            To = user.Email,
                            Body = $"",
                            Subject = "Confirm Registration",
                            Otp = otp,
                            FirstName = $"{request.FirstName}",
                            LastName = $"{request.LastName}",
                            Url = $"{_applicationUrlSettings.ConfirmEmailUrl}{request.Email}?code={otp}",
                        }, templatePath);

                        return new Response<string>("Check your mail for your OTP to verify your email",
                            message: $"User registered successfully.");
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        throw new ApiException("Something went wrong while profiling user");
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email} is already registered.");
            }
        }

        public async Task<Response<bool>> VerifyUser(int otp)
        {
            var user = await _userProfile.GetUserByOtpAsync(otp);

            if (user == null) return new Response<bool>(false, "unsuccessful");

            if (user.VerificationCode == 0)
            {
                throw new ApiException("This link has expired.");
            }

            if (user.VerificationCode != otp)
            {
                throw new ApiException("Invalid verification link!");
            }

            user.VerificationCode = 0;

            await _userProfile.UpdateAsync(user);

            var aspUser = await _userManager.FindByEmailAsync(user.Email);
            aspUser.EmailConfirmed = true;

            await _userManager.UpdateAsync(aspUser);

            return new Response<bool>(true, "user verified successfully");
        }

        //not in use
        public async Task<Response<bool>> VerifyOtp(int otp)
        {
            var user = await _userProfile.GetUserByOtpAsync(otp);

            if (user == null) return new Response<bool>(false, "Invalid OTP!");

            if (user.VerificationCode == 0)
            {
                return new Response<bool>(false, "Used OTP!");
            }

            if (user.VerificationCode != otp)
            {
                return new Response<bool>(false, "Incorrect OTP!");
            }

            return new Response<bool>(true, "Valid OTP");
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, bool isOffline = false)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ApiException($"No Registered Account with {request.Email}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
            }

            var userProfile = await _userProfile.GetUserByEmailAsync(user.Email);

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, isOffline);
            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                UserId = userProfile.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.TokenExpires = jwtSecurityToken.ValidTo;

            if (user.RefreshTokens != null && user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                response.RefreshToken = activeRefreshToken.Token;
                response.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                response.RefreshToken = refreshToken.Token;
                response.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();
            }

            return new Response<AuthenticationResponse>(response, $"User {user.UserName} Authenticated");
        }

        //public async Task<Response<bool>> LogOut(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        throw new ApiException($"No Registered Account with {email}.");
        //    }


        //}

        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new ApiException($"Token did not match any users.");
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                throw new ApiException($"Token Not Active.");
            }

            var userProfile = _userProfile.GetUserByEmailAsync(user.Email);

            //Revoke Current Refresh Token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                UserId = userProfile.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.TokenExpires = jwtSecurityToken.ValidTo;
            response.RefreshToken = newRefreshToken.Token;
            response.RefreshTokenExpiration = newRefreshToken.Expires;

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        public async Task<Response<bool>> ForgotPassword(ForgotPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var userProfile = _userProfile.GetUserByEmailAsync(user.Email).Result;

            if (user == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

            var role = await _userManager.GetRolesAsync(user);

            var url = await GenerateForgotPasswordUrl(user, role.FirstOrDefault());
            var templatePath = "EmailTemplate/ResetPassword.cshtml";

            await _emailService.SendFluentEmailTemplate(new EmailRequest()
            {
                //To = "ambrozzi@filevino.com",
                To = user.Email,
                // To = "marioonosnorbert@gmail.com",
                Body = $"",
                Subject = "Reset Password",
                FirstName = $"{userProfile.FirstName}",
                LastName = $"{userProfile.LastName}",
                Url = url
            }, templatePath);

            return new Response<bool>(true, message: $"Mail sent successfully!");
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(account, code, model.Password);

            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password reset successfully.");
            }
            else
            {
                return new Response<string>(message: String.Join(",", result.Errors.Select(x => x.Description)));
            }
        }

        //public async Task<Response<string>> ChangePassword(ChangePasswordRequest model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);

        //    if (user == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

        //    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        //    if (result.Succeeded)
        //    {
        //        return new Response<string>(model.Email, message: $"Password changed successfully.");
        //    }
        //    else
        //    {
        //        return new Response<string>(message: String.Join(",", result.Errors.Select(x => x.Description)));
        //    }
        //}

        public async Task<Response<string>> ChangePassword(ChangePasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new ApiException($"No Accounts Registered with {model.Email}.");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return new Response<string>(model.Email, succeeded: true, message: "Password changed successfully.");
            }
            else
            {
                return new Response<string>(message: String.Join(",", result.Errors.Select(x => x.Description)), succeeded: false);
            }
        }


        public List<int> GetUserIdsByRoleAsync(string role)
        {
            var aspUsersEmail = _userManager.GetUsersInRoleAsync(role).Result.Select(x => x.Email).ToList();
            var userIds = _userProfile.GetUserIdsByEmail(aspUsersEmail).ToList();

            return userIds;
        }

        public async Task<Response<string>> ResendVerificationMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) throw new ApiException($"{email} is not registered yet.");

            if (user.EmailConfirmed == true) throw new ApiException($"{email} has already been confirmed.");

            var userProfile = await _userProfile.GetUserByEmailAsync(user.Email);

            if (userProfile != null)
            {
                var otp = GenerateOTP();

                userProfile.VerificationCode = otp;

                await _userProfile.UpdateAsync(userProfile);

                var templatePath = "EmailTemplate/ConfirmEmail.cshtml";

                await _emailService.SendFluentEmailTemplate(new EmailRequest()
                {
                    To = userProfile.Email,
                    Body = $"",
                    Subject = "Confirm Registration",
                    Otp = otp,
                    FirstName = $"{userProfile.FirstName}",
                    LastName = $"{userProfile.LastName}",
                    Url = $"{_applicationUrlSettings.ConfirmEmailUrl}{userProfile.Email}?code={otp}",
                }, templatePath);

                return new Response<string>($"A verification mail has been re-sent to {userProfile.Email}. Click on the link to confirm your email.",
                    message: "successful");
            }
            else
            {
                throw new ApiException($"Profile not found for {email}");
            }
        }

        public async Task<string> GetUserRoleByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            string rolename = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return rolename;
        }

        public async Task<string> GetUserRoleById(int userId)
        {
            var userprofile = await _userProfile.GetByIdAsync(userId);
            var user = await _userManager.FindByEmailAsync(userprofile.Email);
            string rolename = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return rolename;
        }

        public Response<AuthenticationResponse> PeriodicAuthentication(AuthenticationRequest request)
        {
            var user = _userManager.FindByEmailAsync(request.Email).Result;

            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var resp = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            //var result = _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false).Result;

            if (resp == PasswordVerificationResult.Failed)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }

            var response = new AuthenticationResponse { UserName = user.UserName };

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                //CreatedByIp = ipAddress
            };
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user, bool isOffline = false)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userProfile = await _userProfile.GetUserByEmailAsync(user.Email);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", userProfile.Id.ToString()),
                new Claim("rol", roles.FirstOrDefault()),
                //new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            //var jwtSecurityToken = new JwtSecurityToken(
            //   issuer: _jwtSettings.Issuer,
            //   audience: _jwtSettings.Audience,
            //   claims: claims,
            //   expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            //   signingCredentials: signingCredentials);

            if (isOffline == true)
            {
                var jwtSecurityToken = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: claims,
               expires: DateTime.UtcNow.AddDays(3650), //offline token would last for 10 years 😂 i.e it won't expire!
               signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }
            else
            {
                var jwtSecurityToken = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
               signingCredentials: signingCredentials);

                return jwtSecurityToken;
            }

            //return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        private int GenerateOTP()
        {
            int min = 1000;
            int max = 9999;
            var random = new Random();
            var otp = random.Next(min, max);

            return otp;
        }

        private async Task<string> GenerateForgotPasswordUrl(ApplicationUser user, string role)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var _enpointUri = new Uri(_applicationUrlSettings.ForgetPasswordUrl);
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "email", user.Email);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "role", role);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        //To Create Supervisors
        public async Task<Response<string>> CreateAdmin(RegisterRequest request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.Email);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Email '{request.Email}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());

                    //use the request to create a new user profile
                    var userProfile = new UserProfile
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        VerificationCode = 0
                    };

                    var resp = await _userProfile.AddAsync(userProfile);

                    if (resp.Id > 0)
                    {
                        return new Response<string>("User registered successfully.");
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        throw new ApiException("Something went wrong while profiling user");
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email} is already registered.");
            }
        }

        //To Get All Users
        public List<int> GetUsersAsync()
        {
            var aspUsersEmail = _userManager.GetUsersInRoleAsync(Roles.User.ToString()).Result.Select(x => x.Email).ToList();
            var userIds = _userProfile.GetUserIdsByEmail(aspUsersEmail).ToList();

            return userIds;
        }
        //To Get All Admimn
        public List<int> GetAdminsAsync()
        {
            var aspUsersEmail = _userManager.GetUsersInRoleAsync(Roles.Admin.ToString()).Result.Select(x => x.Email).ToList();
            var userIds = _userProfile.GetUserIdsByEmail(aspUsersEmail).ToList();

            return userIds;
        }

    }
}
