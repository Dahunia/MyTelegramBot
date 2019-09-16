using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Checkers.Messages;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Checkers.Callback
{
    public class AbstractCallbackChecker : BaseChecker, ICallbackChecker
    {
        private ICallbackChecker _nextChecker;

        public AbstractCallbackChecker(IServiceProvider provider)//ILoggerFactory loggerFactory, IMyLogger filelogger, ITelegramApiRequest telegramRequest) 
            : base(provider)//loggerFactory, filelogger, telegramRequest
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