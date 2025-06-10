using System.Collections.Generic;
using System.Threading.Tasks;
using project_API.Model;

namespace project_API.Repositories.Interface;


    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products> GetByIdAsync(int id);
        Task AddAsync(Products product);
        Task UpdateAsync(Products product);
        
        Task<List<Products>> GetTopSellingProductsAsync(int count);
        Task DeleteAsync(int id);
    }


