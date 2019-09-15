using System.Threading.Tasks;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Data.Interface
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, Chat chat);
        Task<User> Login(string user, string chat);
        Task<User> UserExists(string username, string chatId);
    }
}