using CMS.ApplicationCore.DTO;
using CMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.Service
{
    public interface IAuthService
    {
        public string ConfirmUrl { get; set; }
        public Task CreateUser(RegisterRequest request);
        public Task<bool> CheckExistEmail(string email);
        public Task<Guid> RegisterAsync(RegisterRequest request);
        public Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        public Task<AppUser> GetUserByUserEmail(string email);
        public Task<bool> ResetPasswordAsync(ResetPasswordRequest request);




        public Task<bool> Login(LoginRequest request);
        public Task Logout();
        //public Task<string> GetTokenResetPassword(ForgotPasswordRequest request);
        //public Task<string> GetTokenVerifyEmail(AppUser user);
        public Task<AppUser> GetUserByUserId(string userId);
        public Task<bool> ConfirmEmail(string userId, string token);
    }
}
