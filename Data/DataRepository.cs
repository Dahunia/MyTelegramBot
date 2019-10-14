using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;
using Microsoft.Extensions.Caching.Memory;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Data
{
    public class DataRepository : BaseRepository, IDataRepository
    {
        private readonly IMemoryCache _cache;
        //private readonly DataContext _context;

        public DataRepository(DataContext context, IMemoryCache cache) 
        : base(context) => 
        (_cache) = (cache);
        public async Task<User> GetUser(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
              public async Task<IQueryable<Category>> GetCategories(
            int parent = 0, string languageCode="ru")
        {
            return await Task.Run(() => {
                var categories = _context.Categories
                    .Where(c => c.LanguageCode == languageCode && c.Parent == parent);

                return categories;
            });
        }
        public async Task<IQueryable<Product>> GetProducts(
            int categoryId, string languageCode="ru")
        {
             return await Task.Run(() => {
                var products = _context.Products
                    .Where(p => p.CategoryId == categoryId && p.LanguageCode == languageCode);

                return products;
            });
        }
        public async Task<IQueryable<Category>> GetAllCategories()
        {
            return await Task.Run(() => {
               /*  var categories = _context.Categories
                    .Where(c => c.Parent == parent);

                return categories; */
                return _context.Categories;
            });
        }
        public async Task<Category> GetCategory(int id) => 
            await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Product> GetProduct(int id) =>
            await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> GetFirstProduct() => await _context.Products.FirstAsync();

        public async Task<bool> MessageExists(long messageId)
        {
            if (await _context.Messages.AnyAsync(m => m.Id == messageId))
                return true;
            return false;
        }
        public async Task<bool> UpdateExists(long updateId)
        {
            if (await _context.Updates.AnyAsync(u => u.Id == updateId))
                return true;
            return false;
        }
        public async Task<bool> CallbackExists(string callbackId)
        {
            if (await _context.CallbackQueries.AnyAsync(c => string.Equals(c.Id, callbackId)))
                return true;
            return false;
        }
        private MemoryCacheEntryOptions GetCacheEntryOptions(int time) =>
            new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(5)
            };
    }
}

   /*      public EntityEntry<T> Add<T>(T entity) where T : class => _context.Add(entity);
        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;
        public void Delete<T>(T entity) where T : class => _context.Remove(entity);
 */