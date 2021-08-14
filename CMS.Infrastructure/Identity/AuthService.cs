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

namespace CMS.Infrastructure
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public async void CreateUser(RegisterRequest request)
        {
            AppUser appUser = new AppUser {
                Email = request.Email,
                UserName = request.Email
            };
            var isCreated = await _userManager.CreateAsync(appUser, request.Password);
            if (isCreated.Succeeded)
            { 
            }
        }

        public async Task<Guid> RegisterAsync(RegisterRequest request)
        {
            Guid userId = Guid.Empty;
            
            var user = new AppUser {
                Email = request.Email,
                UserName = request.Email
            };
            var userResult = await _userManager.CreateAsync(user, request.Password);

            if (userResult.Succeeded)
            {
                //SendVerificationEmail(user);
                userId = user.Id;
            }
            return userId;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return true;
            else
                return false;
        }

        public async Task<bool> Login(LoginRequest request)
        {
            bool isLoginSuccess = false;
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var singInStatus = await _signInManager.PasswordSignInAsync(user, request.Password, request.Remember, request.Lock);
                if (singInStatus.Succeeded)
                    isLoginSuccess = true;
            }

            return isLoginSuccess;
        }

        public void Logout()
        {
            _signInManager.SignOutAsync();
        }
        public async Task<string> GetTokenResetPassword(ForgotPasswordRequest request)
        {
            string code = string.Empty;
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                code = await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            return code;
        }

        public async Task<string> GetTokenVerifyEmail(AppUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<AppUser> GetUserByUserId(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        public async Task<bool> CheckExistEmail(string email)
        {
            bool isExist = false;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                isExist = true;

            return isExist;
        }


        //private async void SendVerificationEmail(AppUser user)
        //{
        //    var code = Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(user));
        //    var url = new Uri($"http://localhost:63941/{Controller}/{Action}?userId={user.Id}&code={code}");// &returnUrl={ReturnUrl}";

        //    string clientId = "1035158221116-qv9p42ldlbcljjsc95a1058mp4tuv2vt.apps.googleusercontent.com";
        //    string client_secret = "D95ItqksMp9-vauoLQqvhAag";
        //    string from = "person.info.dev@gmail.com";
        //    string to = user.Email;
        //    string subject = "Test Email";
        //    string content = url.ToString();
        //    IEmailSender email = new GoogleEmailSender(clientId, client_secret, from, to, subject, content);
        //    email.Send();
        //}


    }
}
