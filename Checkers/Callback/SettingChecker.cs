using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using MyTelegramBot.Views.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Checkers.Callback
{
    public class SettingChecker : AbstractCallbackChecker
    {
        private readonly IMyLogger<SettingChecker> _logger;
        private readonly IDataRepository _repo;
        private readonly ITelegramView _view;
        private readonly ITelegramApiRequest _telegramRequest;
         private readonly string[] commands = {@"/about", @"/?"};
        public SettingChecker(
            IMyLogger<SettingChecker> logger,
            ITelegramApiRequest telegramApiRequest,
            IDataRepository repo,
            ITelegramView view)
        { 
            _logger = logger;
            _repo = repo; 
            _view = view;
            _telegramRequest = telegramApiRequest;
        }
        public override async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            var command = "";
        
            MessageTextForEditDto messageTextForEdit = null;
            if (commands.Contains(command = incomingCallbackDto.Data.ToLower()))
            {
                switch(command)
                {   
                    case @"/?":
                        messageTextForEdit = 
                            await _view.Question(incomingCallbackDto.Message);
                        break;
                    case @"/about": 
                        messageTextForEdit = 
                            await _view.About(incomingCallbackDto.Message);
                        break;
                }
                //var answerForSend = await _view.MessageTextForEdit(incomingCallbackDto);                         
                var response = await _telegramRequest.ChangeMessage(messageTextForEdit);
                await _logger.LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(messageTextForEdit));
                return response;
            }
            return await base.Checker(incomingCallbackDto);
        }
    }
}
//var response = await _telegramRequest.SendCallback(answerForSend);
/*  var answerForSend= new AnswerCallbackQueryDto 
{
    callback_query_id = incomingCallbackDto.Id,
    text = "Waiting..."
    //Sample Ожидание... или Ваши данные переданы
}; */