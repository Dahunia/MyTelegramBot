using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Checkers.Messages
{
    public class AbstractMessageChecker : BaseChecker, IMessageChecker
    {
        private IMessageChecker _nextChecker;

        public AbstractMessageChecker(
            ILogger logger,
            IMyLogger filelogger,
            ITelegramApiRequest telegramApiRequest)
            : base(logger, filelogger, telegramApiRequest)
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