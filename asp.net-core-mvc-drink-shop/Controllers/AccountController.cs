using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.ViewModels;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace asp_net_core_mvc_drink_shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
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
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(loginViewModel.UserName);
            }

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
            ModelState.AddModelError(string.Empty, "Username/Password combination not found.");
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
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            ViewBag.Title = "ASP.NET Drinks - Register";
            if (registerViewModel.Password.Equals(registerViewModel.ConfirmPassword) == false)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password must match.");
            }
            if (ModelState.IsValid)
            {
                var user = new AppUser() { UserName = registerViewModel.UserName, Email = registerViewModel.Email };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    _ = await _signInManager.PasswordSignInAsync(user, registerViewModel.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(registerViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            ViewBag.Title = "ASP.NET Drinks - Logout";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ViewBag.Title = "ASP.NET Drinks - My Profile";
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var myProfileViewModel = new MyProfileViewModel { UserName = user.UserName, Email = user.Email };
            return View(myProfileViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Orders()
        {
            ViewBag.Title = "ASP.NET Drinks - My Orders";
            var orders = await _orderRepository.GetUserOrders(_userManager.GetUserId(HttpContext.User));
            var orderListViewModel  = new OrderListViewModel{ Orders = orders };
            return View(orderListViewModel);
        }

        [Authorize]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            ViewBag.Title = "ASP.NET Drinks - Order: " + orderId;
            var order = await _orderRepository.GetOrderById(orderId);
            if (order is null)
            {
                return NotFound();
            }
            if (order?.UserId != _userManager.GetUserId(HttpContext.User))
            {
                return Forbid();
            }
            var orderDetailsViewModel = new OrderDetailsViewModel { Order = order };
            return View(orderDetailsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Settings()
        {
            ViewBag.Title = "ASP.NET Drinks - Account Settings";
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            AccountSettingsViewModel accountSettingsViewModel = new AccountSettingsViewModel
            {
                AddressLine = user.AddressLine,
                UserName = user.UserName,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PublicInfo = user.PublicInfo,
                ZipCode = user.ZipCode,
                State = user.State
            };
            return View(accountSettingsViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Settings(AccountSettingsViewModel accountSettingsViewModel)
        {
            ViewBag.Title = "ASP.NET Drinks - Account Settings";
            if (accountSettingsViewModel.Password != accountSettingsViewModel.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password must match.");
            }

            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            user.FirstName = accountSettingsViewModel.FirstName;
            user.LastName = accountSettingsViewModel.LastName;
            user.PublicInfo = accountSettingsViewModel.PublicInfo;
            user.Country = accountSettingsViewModel.Country;
            user.ZipCode = accountSettingsViewModel.ZipCode;
            user.State = accountSettingsViewModel.State;
            user.City = accountSettingsViewModel.City;
            user.AddressLine = accountSettingsViewModel.AddressLine;
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded == false)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                accountSettingsViewModel.Email = user.Email;
            }


            var emailResult = await _userManager.SetEmailAsync(user, accountSettingsViewModel.Email);
            if (emailResult.Succeeded == false)
            {
                foreach (var error in emailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                accountSettingsViewModel.Email = user.Email;
            }
            var phoneResult = await _userManager.SetPhoneNumberAsync(user, accountSettingsViewModel.PhoneNumber);
            if (phoneResult.Succeeded == false)
            {
                foreach (var error in phoneResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                accountSettingsViewModel.PhoneNumber = user.PhoneNumber;
            }
            if (string.IsNullOrEmpty(accountSettingsViewModel.Password) == false)
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, accountSettingsViewModel.CurrentPassword, accountSettingsViewModel.Password);
                if (passwordResult.Succeeded == false)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    accountSettingsViewModel.UserName = user.UserName;
                }

            }

            return View(accountSettingsViewModel);
        }
    }
}
