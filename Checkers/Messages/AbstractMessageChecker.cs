using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MyTelegramBot.Checkers.Messages
{
    public abstract class AbstractMessageChecker : IMessageChecker
    {
        private IMessageChecker _nextChecker;

        public IMessageChecker SetNext(IMessageChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<string> Checker(MessageDto incomingMessageDto)
        {
            if (this._nextChecker != null)
            {
                return await this._nextChecker.Checker(incomingMessageDto);
            }
            else {
                return "";
            }
        }
    }
}