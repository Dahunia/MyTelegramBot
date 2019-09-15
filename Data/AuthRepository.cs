using System.Threading.Tasks;
using MyTelegramBot.Data.Interface;
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

        public Task<User> Register(User user, Chat chat)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> UserExists(string username, string chatId)
        {
            throw new System.NotImplementedException();
        }
    }
}