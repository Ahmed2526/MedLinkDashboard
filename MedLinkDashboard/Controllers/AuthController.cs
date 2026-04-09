using MedLinkDashboard.Enums;
using MedLinkDashboard.IService;
using MedLinkDashboard.Models;
using MedLinkDashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedLinkDashboard.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        // handle confim email
        //push to github
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
                return View(userVM);

            var user = new ApplicationUser()
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
                PhoneNumber = userVM.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(userVM);
            }

            //Assign default role "Patient"
            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            //Sign in after registration
            await _signInManager.SignInAsync(user, userVM.RememberMe);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginVM());
        }


        //i messed with lockout enabled
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM userVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
                return View(userVM);

            var user = await _userManager.FindByEmailAsync(userVM.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or password");
                return View(userVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.RememberMe, false);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked due to multiple failed attempts. Try again later.");
                return View(userVM);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or password");
                return View(userVM);
            }

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var profileVM = new ProfileVM
            {
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Role = roles.ToList()
            };

            return View(profileVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult forgetpassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //handle if send failed
        public async Task<IActionResult> forgetpassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
                return View("LinkSentSuccessfully");

            //Generate reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Generate reset link
            var resetLink = Url.Action("ResetPassword", "Auth", new { token, email = Email }, Request.Scheme);

            string subject = "MedLink - Password Reset";

            var status = await _emailService.SendEmailAsync(Email, subject, resetLink!);

            return View("LinkSentSuccessfully");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
                return BadRequest("Invalid password reset request.");

            var model = new ResetPasswordVM()
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid User");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return View("ResetPasswordConfirmed");
            }
            foreach (var error in result.Errors)
            {
                if (error.Code == "InvalidToken")
                    ModelState.AddModelError(string.Empty, "This reset link has expired. Please request a new one.");
                else
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckUniqueUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
                return Json(true);

            return Json(false);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckUniqueEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user is null)
                return Json(true);

            return Json(false);
        }

    }
}
