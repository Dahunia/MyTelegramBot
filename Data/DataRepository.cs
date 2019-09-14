using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public DataRepository(DataContext context) => _context = context;
        public void Add<T>(T entity) where T : class => _context.Add(entity);

        public void Delete<T>(T entity) where T : class => _context.Remove(entity);
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _context.Categories.Include( c => c.Name)
                .ToListAsync();
            return categories;
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products.Include(p => p.Name)
                .ToListAsync();
            
            return products;
        }
        public async Task<bool> SaveAll() => await _context.SaveChangesAsync() > 0;
    }
}