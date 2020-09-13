using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_core_mvc_drink_shop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace asp_net_core_mvc_drink_shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.Title = "ASP.NET Drinks - Login";
            if ((HttpContext.User != null) && HttpContext.User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "/";
                }
                return Redirect(returnUrl);
            }
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            ViewBag.Title = "ASP.NET Drinks - Login";
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                    {
                        return RedirectToAction(loginViewModel.ReturnUrl);
                    }
                    else
                    {
                        return Redirect(loginViewModel.ReturnUrl);
                    }
                }
            }
            ModelState.AddModelError("error1", "Username/Password combination not found.");
            return View(loginViewModel);

        }

        public IActionResult Register()
        {
            ViewBag.Title = "ASP.NET Drinks - Register";
            if ((HttpContext.User != null) && HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            ViewBag.Title = "ASP.NET Drinks - Register";
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = loginViewModel.UserName, Email = loginViewModel.UserName };
                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                if (result.Succeeded)
                {
                    _ = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(loginViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            ViewBag.Title = "ASP.NET Drinks - Logout";
            return RedirectToAction("Index", "Home");
        }
    }
}
