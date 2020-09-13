using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.Data.Models;
using asp_net_core_mvc_drink_shop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Controllers
{
    public class DrinkController:Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDrinkRepository _drinkRepository;

        public DrinkController(IDrinkRepository drinkRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _drinkRepository = drinkRepository;

        }

        public ViewResult List(string categoryName)
        {
            string _category = categoryName;
            IEnumerable<Drink> drinks;

            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(categoryName))
            {
                drinks = _drinkRepository.Drinks.OrderBy(n => n.DrinkId);
                currentCategory = "All Drinks";
            }
            else
            {
                if (string.Equals("Alcoholic", _category, StringComparison.OrdinalIgnoreCase))
                {
                    drinks = _drinkRepository.Drinks.Where(p => p.Category.CategoryName.Equals("Alcoholic")).OrderBy(p => p.Name);
                }
                else
                {
                    drinks = _drinkRepository.Drinks.Where(p => p.Category.CategoryName.Equals("Non-alcoholic")).OrderBy(p => p.Name);
                }
                currentCategory = _category;

            }
            DrinkListViewModel drinkListViewModel = new DrinkListViewModel() { Drinks = drinks, CurrentCategory = currentCategory };
            ViewBag.Title = "ASP.NET Drinks - " + currentCategory;

            return View(drinkListViewModel);
        }
    }
}
