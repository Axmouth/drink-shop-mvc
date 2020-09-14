using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.Data.Models;
using asp_net_core_mvc_drink_shop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace asp_net_core_mvc_drink_shop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IDrinkRepository drinkRepository, ShoppingCart shoppingCart)
        {
            _drinkRepository = drinkRepository;
            _shoppingCart = shoppingCart;
        }
        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            ViewBag.Title = "ASP.NET Drinks - Shopping Cart";

            var sCVM = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(sCVM);
        }

        public IActionResult AddToShoppingCart(int drinkId, string returnUrl)
        {
            var selectedDrink = _drinkRepository.Drinks.FirstOrDefault(p => p.DrinkId == drinkId);
            returnUrl ??= Request.Headers["Referer"].ToString() ?? "/";
            ViewBag.Title = "ASP.NET Drinks - Add To Shopping Cart";

            if (selectedDrink != null)
            {
                _shoppingCart.AddToCart(selectedDrink, 1);
            }
            return Redirect(returnUrl);
        }

        public IActionResult RemoveFromShoppingCart(int drinkId, string returnUrl)
        {
            var selectedDrink = _drinkRepository.Drinks.FirstOrDefault(p => p.DrinkId == drinkId);
            returnUrl ??= Request.Headers["Referer"].ToString() ?? "/";
            ViewBag.Title = "ASP.NET Drinks - Remove From Shopping Cart";

            if (selectedDrink != null)
            {
                _shoppingCart.RemoveFromCart(selectedDrink);
            }
            return Redirect(returnUrl);
        }
    }
}