using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Helpers;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Checkers.Messages;
using MyTelegramBot.Checkers.Callback;

namespace MyTelegramBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IMyLogger _filelogger;
        //private readonly TelegramSettings _telegramConfig;
        //private readonly ITelegramApiRequest _telegramRequest;
        private readonly IMessageChecker _messageChecker;
        private readonly ICallbackChecker _callbackChecker;
        public WebhookController(
             IServiceProvider provider
            //,IChecker checker
            ,ILogger<WebhookController> logger
            ,IMyLogger filelogger)
        {
            _logger = logger;
            _filelogger = filelogger; 
            //_checker = checker;

            _messageChecker = provider.GetService<SimpleCommandChecker>();
            _callbackChecker = provider.GetService<CallbackChecker>();
            
            //_messageChecker = callbackChecker
            //  .SetNext(simpleChecker);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IncomingRequestDto incomingRequestDto)
        {
            await LogInformation("INPUT REQUEST \n" + HttpContext.Request.ReadRequestBody());

            object checkResult;
            if (incomingRequestDto.message != null) {
                checkResult = _messageChecker.Checker(incomingRequestDto.message);
            }
            if (incomingRequestDto.callback_query != null) {
                checkResult = _callbackChecker.Checker(incomingRequestDto.callback_query);
            }

            return StatusCode(201);

        }
        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}