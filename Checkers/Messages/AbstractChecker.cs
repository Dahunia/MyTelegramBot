using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Dtos.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class AbstractChecker : IChecker
    {
        private IChecker _nextChecker;
        protected readonly ILogger<AbstractChecker> _logger;
        protected readonly IMyLogger _filelogger;
        protected readonly ITelegramApiRequest _telegramRequest;
        public AbstractChecker(
            ILoggerFactory loggerFactory, 
            IMyLogger filelogger,
            ITelegramApiRequest telegramRequest)
        {
            _logger = loggerFactory.CreateLogger<AbstractChecker>();
            _filelogger = filelogger;
            _telegramRequest = telegramRequest;
        }
        public IChecker SetNext(IChecker checker)
        {
            _nextChecker = checker;

            return checker;
        }
        public virtual async Task<object> Checker(IncomingRequestDto incomingRequest)
        {
            if (this._nextChecker != null)
            {
                return this._nextChecker.Checker(incomingRequest);
            }
            else {
                return null;
            }
        }
        protected async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}