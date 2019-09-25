using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Checkers.Callback
{
    public class AbstractCallbackChecker : BaseChecker, ICallbackChecker
    {
        private ICallbackChecker _nextChecker;

        public AbstractCallbackChecker(
            ILogger logger,
            IMyLogger filelogger,
            ITelegramApiRequest telegramApiRequest
        )
            : base(logger, filelogger, telegramApiRequest)
        {}

        public ICallbackChecker SetNext(ICallbackChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<object> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (this._nextChecker != null)
            {
                return await this._nextChecker.Checker(incomingCallbackDto);
            }
            else {
                return null;
            }
        }
    }
}