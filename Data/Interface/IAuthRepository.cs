using System.Threading.Tasks;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data.Interface
{
    public interface IAuthRepository
    {
        Task<User> Register(User user);
        Task<Chat> Register(Chat chat);
        Task<User> Login(string user, string chat);
        Task<bool> UserExists(long userId);
    }
}