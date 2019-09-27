using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Callback
{
    public interface ICallbackChecker
    {
         ICallbackChecker SetNext(ICallbackChecker checker);

         Task<string> Checker(CallbackQueryDto callbackForCreationDto);
    }
}