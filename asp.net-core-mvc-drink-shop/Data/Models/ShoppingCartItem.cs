using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Data.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }

        public Drink Drink { get; set; }

        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
