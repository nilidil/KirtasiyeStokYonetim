using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<List<Product>> GetProductsWithCategoryAndFeature()
        {
            return await _context.Products.Include(x => x.Category).Include(x=>x.ProductFeature).ToListAsync();
        }
        public async Task<Product> GetProductWithCategoryAndFeature(int id)
        {
            return await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductFeature)
                .FirstOrDefaultAsync(x=>x.Id==id);
        }


    }
}
