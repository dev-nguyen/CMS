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

        public Task<bool> CheckExistEmail(string email);
        public Task<Guid> RegisterAsync(RegisterRequest request);        
        public Task<bool> Login(LoginRequest request);
        public void Logout();
        public Task<string> GetTokenResetPassword(ForgotPasswordRequest request);
        public Task<string> GetTokenVerifyEmail(AppUser user);
        public Task<AppUser> GetUserByUserId(Guid userId);
        public Task<bool> ConfirmEmail(string userId, string code);
    }
}
