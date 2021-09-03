using CMS.ApplicationCore;
using CMS.ApplicationCore.DTO;
using CMS.ApplicationCore.Service;
using CMS.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Infrastructure
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _httpContextAccessor = httpContextAccessor;
        }

        public string ConfirmUrl { get; set; }

        #region Register

        public async Task CreateUserAsync(RegisterRequest request)
        {
            var appUser = new AppUser
            {
                Email = request.Email,
                UserName = request.Email
            };
            var isCreated = await _userManager.CreateAsync(appUser, request.Password);
            if (isCreated.Succeeded)
            {
                var token = Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(appUser));
                string schema = _httpContextAccessor.HttpContext.Request.Scheme;
                string host = _httpContextAccessor.HttpContext.Request.Host.ToString();
                var userId = appUser.Id;
                var activeUrl = $"{schema}://{host}{this.ConfirmUrl}?userId={userId}&token={token}";

                EmailConfig email = new EmailConfig();
                email.To = new string[] { appUser.Email };
                email.Subject = "Active account";
                email.Body = $"Please click on the link below to active your account. <p> <a href=\"{activeUrl}\"> Active </a> </p>";

                IEmailSender sender = new GoogleEmailSender(email, "1035158221116-qv9p42ldlbcljjsc95a1058mp4tuv2vt.apps.googleusercontent.com", "D95ItqksMp9-vauoLQqvhAag");
                sender.Send();
            }
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await GetUserByEmailAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return true;
            else
                return false;
        }

        #endregion

        #region Reset Password

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            bool result = false;
            var user = await GetUserByEmailAsync(request.Email);
            var status = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (status.Succeeded)
                result = true;

            return result;
        }

        #endregion

        #region Login Logout
        public async Task<bool> LoginAsync(LoginRequest request)
        {
            bool isLoginSuccess = false;
            var user = await GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                var singInStatus = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, request.Lock);
                if (singInStatus.Succeeded)
                    isLoginSuccess = true;
            }

            return isLoginSuccess;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region Helper Methods

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        #endregion

        #region Ajax call

        public async Task<bool> CheckExistEmailAsync(string email)
        {
            bool isExist = false;
            var user = await GetUserByEmailAsync(email);
            if (user != null)
                isExist = true;

            return isExist;
        }

        #endregion
    }
}
