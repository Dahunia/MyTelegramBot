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
        protected readonly ILogger _logger;
        protected readonly IMyLogger _filelogger;
        protected readonly ITelegramApiRequest _telegramRequest;

        public AbstractMessageChecker(
            ILogger logger,
            IMyLogger filelogger,
            ITelegramApiRequest telegramApiRequest) =>
        (_logger, _filelogger, _telegramRequest) = 
        (logger, filelogger, telegramApiRequest);

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