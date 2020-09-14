using asp_net_core_mvc_drink_shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Data.interfaces
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);

        Task<IEnumerable<Order>> GetUserOrders(string userId);

        Task<Order> GetOrderById(int orderId);
    }
}
