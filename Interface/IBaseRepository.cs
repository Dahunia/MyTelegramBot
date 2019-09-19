using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MyTelegramBot.Interface
{
    public interface IBaseRepository
    {
        EntityEntry<T> Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAllAsync();
    }
}