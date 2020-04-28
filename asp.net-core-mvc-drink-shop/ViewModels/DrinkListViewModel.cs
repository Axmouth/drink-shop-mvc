using asp_net_core_mvc_drink_shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.ViewModels
{
    public class DrinkListViewModel
    {

        public IEnumerable<Drink> Drinks { get; set; }
        public string CurrentCategory { get; set; }

    }
}
