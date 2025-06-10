using project_API.data;
using project_API.Model;
using Microsoft.EntityFrameworkCore;

namespace project_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly appdbcontext _db;

        public ProductRepository(appdbcontext db)
        {
            _db = db;
    
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await _db.Products
                .Include(p => p.Category) 
                .ToListAsync();
        }
        public async Task<List<Products>> GetTopSellingProductsAsync(int count)
        {
            var result = await _db.Order_Items
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .Join(_db.Products,
                    g => g.ProductId,
                    p => p.Id,
                    (g, p) => p)
                .ToListAsync();

            return result;
        }
        


        public async Task<Products?> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task AddAsync(Products product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Products product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }
    }
}