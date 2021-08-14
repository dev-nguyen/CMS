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

        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            return null;
        }



        //// GET: AccountController
        //public ActionResult Login(string returnUrl)
        //{
        //    return View();
        //}

        //public ActionResult Register()
        //{
        //    return View();
        //}

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
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    StatusMessageViewModel model = new StatusMessageViewModel();

        //    var status = await _authService.ConfirmEmail(userId, code);
        //    if (status)
        //    {
        //        model.Code = "200";
        //        model.Message = $"Your account is actived successfully. Click <a href='/Account/Login/'> here </a> to login";
        //    }
        //    return View("StatusMessage", model);
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
