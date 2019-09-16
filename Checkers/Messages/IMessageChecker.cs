using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public interface IMessageChecker
    {
         IMessageChecker SetNext(IMessageChecker handler);
         Task<object> Checker(MessageDto incomingMessageDto);
    }
}