using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Interface
{
    public interface IDataRepository : IBaseRepository
    {
        Task<Category> GetCategory(int id);
        Task<IQueryable<Category>> GetCategories(int parent = 0, string languageCode = "ru");
        Task<IQueryable<Category>> GetAllCategories();//int parent = 0);
        Task<Product> GetProduct(int id);
        Task<Product> GetFirstProduct();
        Task<IQueryable<Product>> GetProducts(int categoryId, string languageCode);
        Task<bool> MessageExists(long messageId);
        Task<bool> UpdateExists(long updateId);
        Task<bool> CallbackExists(string callbackId);
    }
}