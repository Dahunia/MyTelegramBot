using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Work;
using MyTelegramBot.Dtos.Markets.Binance;
using MyTelegramBot.Data.Interface;
using System;
using Microsoft.Extensions.Options;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Models.Settings;
using MyTelegramBot.Models.Telegram;
using System.Collections.Generic;
using Newtonsoft.Json;
using MyTelegramBot.Helpers;
using MyTelegramBot.Dtos.Telegram;
using System.Text.RegularExpressions;
using MyTelegramBot.Checkers.Messages;
using Microsoft.Extensions.DependencyInjection;

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
        private readonly IChecker _checker;
        public WebhookController(
             IServiceProvider provider
            //,IChecker checker
            ,ILogger<WebhookController> logger
            ,IMyLogger filelogger)
        {
            _logger = logger;
            _filelogger = filelogger; 
            //_checker = checker;

            var callbackChecker = provider.GetService<CallbackChecker>();
            var simpleChecker = provider.GetService<SimpleCommandChecker>();
            
            _checker = callbackChecker
                .SetNext(simpleChecker);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IncomingRequestDto incomingRequestDto)
        {
            await LogInformation("INPUT REQUEST \n" + HttpContext.Request.ReadRequestBody());

            var result = _checker.Checker(incomingRequestDto);

            return StatusCode(201);

        }
        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}