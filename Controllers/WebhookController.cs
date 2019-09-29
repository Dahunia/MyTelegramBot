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
        private readonly IMyLogger<WebhookController> _logger;
        private readonly IMessageChecker _messageChecker;
        private readonly ICallbackChecker _callbackChecker;
        private readonly IMapper _mapper;
        private readonly IDataRepository _repo;
        public WebhookController(
            IMyLogger<WebhookController> logger
            ,IMessageChecker messageChecker
            ,ICallbackChecker callbackChecker
            ,IMapper mapper
            ,IDataRepository repo)
        {
            _mapper = mapper;
            _logger = logger; 
            _messageChecker = messageChecker;
            _callbackChecker = callbackChecker;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateForCreationDto incomingRequestDto)
        {
            //var temp = HttpContext.Request.Cookies;
            //Debug information
            var request = HttpContext.Request.ReadRequestBody();
            var requestObject = JsonConvert.DeserializeObject(request);
            await _logger.LogInformation("INCOMING REQUEST\n" + requestObject.GetDump());
            
            string responseReceived = "";
            if (incomingRequestDto.Message != null) 
            {
                responseReceived = await _messageChecker.Checker(incomingRequestDto.Message);
            }
            if (incomingRequestDto.CallbackQuery != null) 
            {    
                responseReceived = await _callbackChecker.Checker(incomingRequestDto.CallbackQuery);
            }

            if (incomingRequestDto != null)
            {
                var updateForCreation = _mapper.Map<Update>(incomingRequestDto);
                _repo.Add(updateForCreation);
                await _repo.SaveAllAsync();
            }
     
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(responseReceived);
            if (responseDto != null) {
                await _logger.LogInformation($"\nReceived RESPONSE after send \n {responseDto.GetDump()}");
                var responseForCreation = _mapper.Map<Response>(responseDto);
                _repo.Add(responseForCreation);
                await _repo.SaveAllAsync();
            }

            return StatusCode(201);//Ok(checkResult);
        }
    }
}

//_messageChecker = provider.GetService<DataChecker>();
//_messageChecker.SetNext( provider.GetService<SimpleCommandChecker>() );
//_callbackChecker = provider.GetService<CallbackChecker>();