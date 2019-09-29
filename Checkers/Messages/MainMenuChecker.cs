using System.Threading.Tasks;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Helpers;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Views.Telegram;

namespace MyTelegramBot.Checkers.Messages
{
    public class MainMenuChecker : AbstractMessageChecker
    {
        private readonly IMyLogger<MainMenuChecker>  _logger;
        private readonly ITelegramView _view;
        private readonly ITelegramApiRequest _telegramRequest;
        //private readonly string[] commands = {"/product", "/cat"};
        public MainMenuChecker(
            IMyLogger<MainMenuChecker> logger,
            ITelegramView view,
            ITelegramApiRequest telegramApiRequest)
        {
            _logger = logger;
            _view = view;
            _telegramRequest = telegramApiRequest;
        }
        public override async Task<string> Checker(MessageDto incomingMessageDto) 
        {
            var command = "";
            var response = "";
            object objForDispay = null;
            if ((command = incomingMessageDto.Text.ToLower()) != null)
            {
                switch(command)
                {
                    case @"/start":
                        var messageForSend = await _view.Start(incomingMessageDto);
                        response = await _telegramRequest.SendMessage(messageForSend);
                        objForDispay = messageForSend;
                        break;
                    default:
                        var defaultMessageForSend = await _view.Default(incomingMessageDto);
                        response = await _telegramRequest.SendMessage(defaultMessageForSend);
                        objForDispay = defaultMessageForSend;
                        break;
                }
              
                await _logger.LogInformation("Was SENT TO USER\n" + objForDispay.GetDump());

                return response;    
            }
            return await base.Checker(incomingMessageDto);
        }
    }
}