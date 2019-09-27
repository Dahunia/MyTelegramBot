using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Interface;
using MyTelegramBot.Helpers;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Checkers.Messages;
using MyTelegramBot.Checkers.Callback;
using Newtonsoft.Json;
using AutoMapper;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IMyLogger _filelogger;
        private readonly IMessageChecker _messageChecker;
        private readonly ICallbackChecker _callbackChecker;
        private readonly IMapper _mapper;
        private readonly IDataRepository _repo;
        public WebhookController(
            //IServiceProvider provider
            IMessageChecker messageChecker
            ,ICallbackChecker callbackChecker
            ,IMapper mapper
            ,IDataRepository repo
            ,ILogger<WebhookController> logger
            ,IMyLogger filelogger)
        {
            _logger = logger;
            _mapper = mapper;
            _filelogger = filelogger; 
            _messageChecker = messageChecker;
            _callbackChecker = callbackChecker;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IncomingRequestDto incomingRequestDto)
        {
            string responseReceived = "";
            if (incomingRequestDto.message != null) 
            {
                responseReceived = await _messageChecker.Checker(incomingRequestDto.message);
            }
            if (incomingRequestDto.callback_query != null) 
            {    
                responseReceived = await _callbackChecker.Checker(incomingRequestDto.callback_query);
            }
            //var temp = HttpContext.Request.Cookies;
            //Debug information
            var request = HttpContext.Request.ReadRequestBody();
            var requestObject = JsonConvert.DeserializeObject(request);
            await LogInformation("INCOMING REQUEST\n" + requestObject.GetDump());

            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(responseReceived);
            if (responseDto != null) {
                await LogInformation($"\nReceived RESPONSE after send \n {responseDto.GetDump()}");
                var responseForCreation = _mapper.Map<Response>(responseDto);
                _repo.Add(responseForCreation);
                await _repo.SaveAllAsync();
            }

            return StatusCode(201);//Ok(checkResult);
        }
        public async Task LogInformation(string message) 
        {
            _logger?.LogInformation(message);
            if (_filelogger != null) {
                await _filelogger.WriteInformationAsync(message);
            }
        }
    }
}

//_messageChecker = provider.GetService<DataChecker>();
//_messageChecker.SetNext( provider.GetService<SimpleCommandChecker>() );
//_callbackChecker = provider.GetService<CallbackChecker>();