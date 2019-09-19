using System.Threading.Tasks;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Interface
{
    public interface IAuthRepository: IBaseRepository
    {
        Task<User> Register(User user);
        Task<Chat> Register(Chat chat);
        Task<User> Login(string user, string chat);
        Task<bool> UserExists(long userId);
        Task<User> GetUser(long userId);
        Task<bool> ChatExists(long chatId);
    }
}