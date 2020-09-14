using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Components
{
    public class CountrySelect: ViewComponent
    {

        public CountrySelect()
        {
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
