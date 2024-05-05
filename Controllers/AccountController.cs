using CBS_ASP.NET_Core_Course_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CBS_ASP.NET_Core_Course_Project.Controllers
{
    public class AccountController : Controller
    {
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
    }
}
