using System.Collections.Generic;
using System.Threading.Tasks;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data.Interface
{
    public interface IDataRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
    }
}