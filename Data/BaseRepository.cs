using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Data
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly DataContext _context;
        public BaseRepository(DataContext context) => _context = context;
        public EntityEntry<T> Add<T>(T entity) where T : class => _context.Add(entity);
        public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;
        public void Delete<T>(T entity) where T : class => _context.Remove(entity);
    }
}