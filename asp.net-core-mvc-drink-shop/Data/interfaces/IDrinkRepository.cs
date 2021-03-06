﻿using asp_net_core_mvc_drink_shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Data.interfaces
{
    public interface IDrinkRepository
    {
        IEnumerable<Drink> Drinks { get;  }

        IEnumerable<Drink> PreferredDrinks { get;  }

        Drink GetDrinkById(int drinkId);
    }
}
