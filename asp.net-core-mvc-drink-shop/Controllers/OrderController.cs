using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.Data.Models;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asp_net_core_mvc_drink_shop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart, UserManager<AppUser> userManager)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> CheckoutAsync()
        {
            ViewBag.Title = "ASP.NET Drinks - Checkout";
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var order = new Order
            {
                AddressLine1 = user.AddressLine,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                State = user.State,
                ZipCode = user.ZipCode
            };
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            ViewBag.Title = "ASP.NET Drinks - Checkout";
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some drinks first!");
            }

            if (ModelState.IsValid)
            {
                order.UserId = _userManager.GetUserId(HttpContext.User);
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order!";
            ViewBag.Title = "ASP.NET Drinks - Checkout Complete";
            return View();
        }
    }
}