using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public interface IChecker
    {
         IChecker SetNext(IChecker handler);

         Task<object> Checker(IncomingRequestDto incomingRequest);
    }
}