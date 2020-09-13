using asp_net_core_mvc_drink_shop.Data.Models;
using System.Collections.Generic;

namespace asp_net_core_mvc_drink_shop.ViewModels
{
    public class DrinksListViewModel
    {
        public IEnumerable<Drink> Drinks { get; set; }
        public string CurrentCategory { get; set; }
    }
}
