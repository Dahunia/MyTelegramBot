using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;

namespace MyTelegramBot.Checkers.Callback
{
    public class AbstractCallbackChecker : ICallbackChecker
    {
        private ICallbackChecker _nextChecker;
        protected readonly ILogger _logger;
        protected readonly IMyLogger _filelogger;
        protected readonly ITelegramApiRequest _telegramRequest;

        public AbstractCallbackChecker(
            ILogger logger,
            IMyLogger filelogger,
            ITelegramApiRequest telegramApiRequest) =>
        (_logger, _filelogger, _telegramRequest) = 
        (logger, filelogger, telegramApiRequest);

        public ICallbackChecker SetNext(ICallbackChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (this._nextChecker != null)
            {
                return await this._nextChecker.Checker(incomingCallbackDto);
            }
            else {
                return "";
            }
        }
        protected async Task LogInformation(string message) 
        {
            _logger?.LogInformation(message);
            if (_filelogger != null)
            {
                await _filelogger.WriteInformationAsync(message);
            }
        }
    }
}