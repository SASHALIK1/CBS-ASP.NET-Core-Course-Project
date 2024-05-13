using CBS_ASP.NET_Core_Course_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CBS_ASP.NET_Core_Course_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CBS_ASP.NET_Core_Course_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailSenderService _emailSenderService;
        private readonly ExchangeRateService _exchangeRateService;
        public AccountController(EmailSenderService emailSenderService, ExchangeRateService exchangeRateService, UserManager<User> userManager)
        {
            _emailSenderService = emailSenderService;
            _exchangeRateService = exchangeRateService;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var userAlreadyExists = Database.Users.Any(x => x.Login == model.Login);
                if (userAlreadyExists)
                {
                    ModelState.AddModelError(nameof(model.Login), "Login is already in use");
                    return View(model);
                }
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Login = model.Login,
                    PasswordHash = PasswordHasher.HashPassword(model.Password)
                };
                Database.Users.Add(newUser);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginBindingModel model)
        {
            var serviceModel = model.ToServiceModel();

            if (ModelState.IsValid)
            {
                var userOrNull = Database.Users.FirstOrDefault(x => x.Login == model.Login);
                if (userOrNull is User user)
                {
                    var isCorrectPassword = PasswordHasher.IsCorrectPassword(user, model.Password);
                    if (isCorrectPassword)
                    {
                        await SignInAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Wrong login or password");
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, "User"),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        public IActionResult Complete()
        {
            ViewBag.Username = User.Identity.Name;
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AccountSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new AccountSettingsViewModel
            {
                Email = user.Login,
                //WantsEmailNotifications = user.WantsEmailNotifications
            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AccountSettings(AccountSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                //user.WantsEmailNotifications = model.WantsEmailNotifications;

                IdentityResult result = IdentityResult.Success;

                if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.CurrentPassword))
                {
                    result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                }
                else
                {
                    result = await _userManager.UpdateAsync(user);
                }

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Налаштування облікового запису успішно оновлено.";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

    }
}
