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

        #region Register
        public Task CreateUserAsync(RegisterRequest request);
        public Task<bool> CheckExistEmailAsync(string email);
        public Task<bool> ConfirmEmailAsync(string userId, string token);
        #endregion

        #region Reset Password
        public Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        
        public Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
        #endregion

        #region Login Logout

        public Task<bool> LoginAsync(LoginRequest request);
        public Task LogoutAsync();

        #endregion

        #region Helper Methods
        public Task<AppUser> GetUserByIdAsync(string userId);
        public Task<AppUser> GetUserByEmailAsync(string email);

        #endregion
    }
}
