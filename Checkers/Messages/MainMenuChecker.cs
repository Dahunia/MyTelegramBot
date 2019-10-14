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
        //private readonly IMyLogger<MainMenuChecker>  _logger;
        private readonly ITelegramView _view;
        //private readonly string[] commands = {"/product", "/cat"};
        public MainMenuChecker(
            IMyLogger<AbstractMessageChecker> logger,
            IBackwardRepository backwardRepository,
            ITelegramView view,
            ITelegramRequest telegramRequest)
        : base(logger, backwardRepository, telegramRequest)
        {
            //_logger = logger;
            _view = view;
        }
        public override async Task<string> Checker(MessageDto incomingMessageDto) 
        {
            var command = "";
            //var response = "";

            if ((command = incomingMessageDto.Text.ToLower()) != null)
            {
                switch(command)
                {
                    case @"/start":
                        var startView = await _view.StartView(incomingMessageDto);
                        return await base.SendView(startView, incomingMessageDto);
/*                         response = await _telegramRequest.SendMessage(startView);
                        await _logger.LogSentToUser(startView); */
                        //break;
                    default:
                        var defaultView = await _view.DefaultView(incomingMessageDto);
                        return await base.SendView(defaultView, incomingMessageDto);
/*                         response = await _telegramRequest.SendMessage(defaultView);
                        await _logger.LogSentToUser(defaultView); */
                        //break;
                }   
            }
            return await base.Checker(incomingMessageDto);
        }
    }
}