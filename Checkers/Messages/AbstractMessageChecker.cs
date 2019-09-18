using System;
using System.Threading.Tasks;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class AbstractMessageChecker : BaseChecker, IMessageChecker
    {
        private IMessageChecker _nextChecker;

        public AbstractMessageChecker(IServiceProvider provider)
            : base(provider)
        {}

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