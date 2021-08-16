using CMS.ApplicationCore;
using CMS.ApplicationCore.DTO;
using CMS.ApplicationCore.Service;
using CMS.Infrastructure;
using CMS.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        IAuthService _authService;
        public AccountController(IAuthService authenticationService)
        {
            _authService = authenticationService;
        }





        //// GET: AccountController
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result)
            {
                return Redirect(request.ReturnUrl);
            }
            return View("/Catalog/Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            var message = GetMessage("200", "You are loged out");
            return View("StatusMessage", message);
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromForm] RegisterRequest request)
        {
            _authService.ConfirmUrl = Url.Action(nameof(ConfirmEmail), "Account");
            await _authService.CreateUser(request);

            var message = GetMessage("200", $"The active link was sent to your email. Please check email to complete registration");            
            return View("StatusMessage", message);
        }

        /// <summary>
        /// userId and code are returned from email active link
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            StatusMessageViewModel message = null;

            var status = await _authService.ConfirmEmail(userId, token);
            if (status)
            {
                message = GetMessage("200", $"Your account is actived successfully. Click <a href='/Account/Login/'> here </a> to login");
            }
            return View("StatusMessage", message);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> ForgotPassword([FromForm]string email)
        {
            var user = await _authService.GetUserByUserEmail(email);
            string token = await _authService.GeneratePasswordResetTokenAsync(user);

            string activeUrl = Url.Action(nameof(ResetPassword), "account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());

            EmailConfig emailMessage = new EmailConfig();
            emailMessage.To = new string[] { user.Email };
            emailMessage.Subject = "Reset Password";
            emailMessage.Body = $"Please click on the link below to reset your password. <p> <a href=\"{activeUrl}\"> Reset password </a> </p>";

            IEmailSender sender = new GoogleEmailSender(emailMessage, "1035158221116-qv9p42ldlbcljjsc95a1058mp4tuv2vt.apps.googleusercontent.com", "D95ItqksMp9-vauoLQqvhAag");
            sender.Send();

            var message = GetMessage("200", $"The reset password link was sent to your email. Please check email to complete reset password");
            return View("StatusMessage", message);
        }
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            var user = await _authService.GetUserByUserId(userId);
            ResetPasswordRequest model = new ResetPasswordRequest
            {
                Token = token,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            StatusMessageViewModel msg = null;
            var result = await _authService.ResetPasswordAsync(request);
            if (result)
            {
                msg = GetMessage("200", "Your password is reset. Please click to <a href=\"/Account/Login\">login</a"); 
            }
            return View("StatusMessage", msg);
        }
        //[HttpPost]
        //public async Task<ActionResult> Register([FromForm] RegisterRequest model)
        //{
        //    var userId = await _authService.RegisterAsync(model);
        //    var user = await _authService.GetUserByUserId(userId);
        //    var token = await _authService.GetTokenVerifyEmail(user);

        //    var url = Url.Action(nameof(ConfirmEmail), "Account", new { userId = userId, code = token }, Request.Scheme, Request.Host.ToString());

        //    EmailConfig email = new EmailConfig();
        //    email.To = new string[] { user.Email };
        //    email.Subject = "Active account";
        //    email.Body = $"Please click on the link below to active your account. <p> <a href=\"{url}\"> Active </a> </p>";

        //    IEmailSender sender = new GoogleEmailSender(email, "1035158221116-qv9p42ldlbcljjsc95a1058mp4tuv2vt.apps.googleusercontent.com", "D95ItqksMp9-vauoLQqvhAag");
        //    sender.Send();

        //    StatusMessageViewModel message = new StatusMessageViewModel();
        //    message.Code = "200";
        //    message.Message = $"The active link was sent to your email. Please check email to complete registration";

        //    return View("StatusMessage", message);
        //}


        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}


        //public IActionResult GoogleLogin()
        //{
        //    string redirectUrl = Url.Action("GoogleResponse", "Account");
        //    Infrastructure.Identity.GoogleService gg = new Infrastructure.Identity.GoogleService(_user)
        //    return Infrastructure.Identity.GoogleService.Login(redirectUrl);
        //    //var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        //    //return new ChallengeResult("Google", properties);
        //}
        //public ActionResult GoogleResponse()

        private StatusMessageViewModel GetMessage(string code, string message)
        {
            return new StatusMessageViewModel { Code = code, Message = message };
        }

        #region Ajax

        [HttpGet]
        public async Task<bool> CheckUserExist([FromQuery] string email)
        {
            var valid = await _authService.CheckExistEmail(email);
            return !valid;
        }
        #endregion
    }
}
