using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using LinkMaker.MVC.Models;
using LinkMaker.Data.Entities.Identity;
using System.Text;
using System.Text.Encodings.Web;

namespace LinkMaker.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<LinkMakerUser> _signInManager;
        private readonly UserManager<LinkMakerUser> _userManager;
        private readonly IUserStore<LinkMakerUser> _userStore;
        private readonly IUserEmailStore<LinkMakerUser> _emailStore;
        private readonly ILogger<RegisterVM> _logger;

        public AuthenticationController(
            UserManager<LinkMakerUser> userManager,
            IUserStore<LinkMakerUser> userStore,
            SignInManager<LinkMakerUser> signInManager,
            ILogger<RegisterVM> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var loginVM = new LoginVM
            {
                ReturnUrl = returnUrl,
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var returnUrl = loginVM.ReturnUrl == null ? Url.Content("~/") : loginVM.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginVM.Email,
                loginVM.Password,
                loginVM.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl!);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = loginVM.RememberMe });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToAction("Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginVM);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            var registerVM = new RegisterVM
            {
                ReturnUrl = returnUrl,
            };
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var returnUrl = string.IsNullOrEmpty(registerVM.ReturnUrl) ? Url.Content("~/") : registerVM.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, registerVM.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, registerVM.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Gán role mặc định nếu m đã seed sẵn role User
                await _userManager.AddToRoleAsync(user, "User");

                // Tạm bỏ email confirm theo note của thầy
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl!);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerVM);
        }

        public IActionResult RegisterConfirmation(string email, string? returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        private IUserEmailStore<LinkMakerUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<LinkMakerUser>)_userStore;
        }

        private LinkMakerUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<LinkMakerUser>();
            }
            catch
            {
                throw new InvalidOperationException(
                    $"Can't create an instance of '{nameof(LinkMakerUser)}'. " +
                    $"Ensure that '{nameof(LinkMakerUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }
    }
}