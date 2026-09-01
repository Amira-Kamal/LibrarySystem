using LibrarySystem.Models;
using LibrarySystem.Repositories;
using LibrarySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers{

    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;


    public AccountController(
            IAccountRepository accountRepository,
            SignInManager<ApplicationUser> signInManager)
            {
                _accountRepository = accountRepository;
                _signInManager = signInManager;
            }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser =
                    await _accountRepository.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(
                        "Email",
                        "This email is already registered.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result =
                    await _accountRepository.CreateUserAsync(
                        user,
                        model.Password);

                if (result.Succeeded)
                {
                    await _accountRepository.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(
                        user,
                        isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(true);

            var existingUser =
                await _accountRepository.FindByEmailAsync(email);

            if (existingUser != null)
                return Json("This email is already registered.");

            return Json(true);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user =
                    await _accountRepository.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var passwordCorrect =
                        await _accountRepository.CheckPasswordAsync(
                            user,
                            model.Password);

                    if (passwordCorrect)
                    {
                        await _signInManager.SignInAsync(
                            user,
                            model.RememberMe);

                        return RedirectToLocal(returnUrl);
                    }
                }

                ModelState.AddModelError(
                    "",
                    "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl)
                && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }


    }

}
