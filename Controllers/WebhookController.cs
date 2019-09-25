using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MyTelegramBot.Interface;
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
            //IServiceProvider provider
            IMessageChecker messageChecker
            ,ILogger<WebhookController> logger
            ,IMyLogger filelogger)
        {
            _logger = logger;
            _filelogger = filelogger; 
            _messageChecker = messageChecker;

            _callbackChecker = null;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IncomingRequestDto incomingRequestDto)
        {
            //TODO new line for new input request not response
            await LogInformation("\nINPUT REQUEST \n" + HttpContext.Request.ReadRequestBody());
            //var temp = HttpContext.Request.Cookies;

            string checkResult = "";
            if (incomingRequestDto.message != null) {
                checkResult = await _messageChecker.Checker(incomingRequestDto.message);
            }
            if (incomingRequestDto.callback_query != null) {
                
                //checkResult = await _callbackChecker.Checker(incomingRequestDto.callback_query);
            }

            await LogInformation($"\nSENT RESPONSE \n {checkResult}");
            return Ok(checkResult);//StatusCode(201);
        }
        public async Task LogInformation(string message) 
        {
            _logger?.LogInformation(message);
            await _filelogger?.WriteInformationAsync(message);
        }
    }
}


//_messageChecker = provider.GetService<DataChecker>();
//_messageChecker.SetNext( provider.GetService<SimpleCommandChecker>() );
//_callbackChecker = provider.GetService<CallbackChecker>();