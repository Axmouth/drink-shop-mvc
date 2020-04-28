using asp_net_core_mvc_drink_shop.Data.interfaces;
using asp_net_core_mvc_drink_shop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_mvc_drink_shop.Data.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public IEnumerable<Category> Categories => _appDbContext.Categories;
    }
}
