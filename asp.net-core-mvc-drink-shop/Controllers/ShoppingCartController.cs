﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.Data.Models;
using asp_net_core_mvc_drink_shop.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

            var sCVM = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(sCVM);
        }

        public RedirectToActionResult AddToShoppingCart(int drinkId)
        {
            var selectedDrink = _drinkRepository.Drinks.FirstOrDefault(p => p.DrinkId == drinkId);

            if (selectedDrink != null)
            {
                _shoppingCart.AddToCart(selectedDrink, 1);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int drinkId)
        {
            var selectedDrink = _drinkRepository.Drinks.FirstOrDefault(p => p.DrinkId == drinkId);

            if (selectedDrink != null)
            {
                _shoppingCart.RemoveFromCart(selectedDrink);
            }

            return RedirectToAction("Index");
        }
    }
}