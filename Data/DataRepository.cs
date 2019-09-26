using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MyTelegramBot.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public DataRepository(DataContext context) => _context = context;
        public EntityEntry<T> Add<T>(T entity) where T : class => _context.Add(entity);
        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;
        public void Delete<T>(T entity) where T : class => _context.Remove(entity);
        public async Task<User> GetUser(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
        public async Task<IEnumerable<Category>> GetCategories(
            string languageCode="ru",
            int parent = 0)
        {
            return await Task.Run(() => {
                var categories = _context.Categories
                    .Where(c => c.LanguageCode == languageCode && c.Parent == parent);

                return categories;
            });
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts(long userId)
        {
            var products = await _context.Products.Include(p => p.Name)
                .ToListAsync();
            
            return products;
        }

        public async Task<bool> MessageExists(long messageId)
        {
            if (await _context.Messages.AnyAsync(m => m.Id == messageId))
                return true;
            return false;
        }
    }
}