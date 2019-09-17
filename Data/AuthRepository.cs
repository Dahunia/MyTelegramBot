using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context) => _context = context;

        public Task<User> Login(string user, string chat)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Register(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

         public async Task<Chat> Register(Chat chat)
        {
            await _context.AddAsync(chat);
            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<bool> UserExists(long userId)
        {
            if (await _context.Users.AnyAsync(u => u.Id == userId))
                return true;
            return false;
        }
        public async Task<bool> ChatExists(long chatId)
        {
            if (await _context.Chats.AnyAsync(u => u.Id == chatId))
                return true;
            return false;
        }
        public async Task<User> GetUser(long userId) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
      
    }
}