using HRManagementSystem.Models;
using HRManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender
            )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
        }
        [HttpGet]
        public IActionResult Login ()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View("Login", loginViewModel);

            var applicationUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (applicationUser == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect.");
                return View("Login", loginViewModel);
            }

            if (!await _userManager.CheckPasswordAsync(applicationUser, loginViewModel.Password))
            {
                ModelState.AddModelError("", "Email or password is incorrect.");
                return View("Login", loginViewModel);
            }

            var claims = new List<Claim>
            {
                new Claim("EmployeeId", applicationUser.EmployeeId.ToString()),
                new Claim("DisplayName", applicationUser.DisplayName)
            };

            await _signInManager.SignInWithClaimsAsync(applicationUser, loginViewModel.RememberMe, claims);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordViewModel);

            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new {token, email = forgotPasswordViewModel.Email},
                Request.Scheme
            );

            await _emailSender.SendEmailAsync(
                 forgotPasswordViewModel.Email,
                 "Reset Password",
                 $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>"
            );
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() => View("ForgotPasswordConfirmation");

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation() => View("ResetPasswordConfirmation");

        [HttpGet]
        [Authorize(Roles = "HR,Admin,Manager,Employee")]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "HR,Admin,Manager,Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                ViewData["SuccessMessage"] = "Password changed successfully!";
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
    }
}
