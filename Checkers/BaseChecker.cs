using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace MyTelegramBot.Checkers
{
    public class BaseChecker
    {
        protected readonly ILogger<BaseChecker> _logger;
        protected readonly IMyLogger _filelogger;
        protected readonly ITelegramApiRequest _telegramRequest;
        public BaseChecker(
            IServiceProvider provider
       /*      ILoggerFactory loggerFactory, 
            IMyLogger filelogger,
            ITelegramApiRequest telegramRequest */)
        {
            //_logger = loggerFactory.CreateLogger<BaseChecker>();
            _logger = provider.GetService<ILoggerFactory>()
                .CreateLogger<BaseChecker>();
            _filelogger = provider.GetService<IMyLogger>();
            _telegramRequest = provider.GetService<ITelegramApiRequest>();
        }
        protected async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}