using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Data.mocks
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> Categories
        {
            get
            {
                return new List<Category>
                {
                    new Category{CategoryName= "Alcoholic", Description = "All alcoholic drinks"},
                    new Category{CategoryName= "Non-Alcoholic", Description = "All non-alcoholic drinks"},
                };
            }
        }
    }
}
