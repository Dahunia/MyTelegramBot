using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using Newtonsoft.Json;

namespace MyTelegramBot.Checkers.Callback
{
    public class CallbackChecker : AbstractCallbackChecker
    {
        public CallbackChecker(
            ILogger<CallbackChecker> logger,
            IMyLogger filelogger,
            ITelegramApiRequest telegramApiRequest)
            : base(logger, filelogger, telegramApiRequest)
        {}
        public override async Task<object> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (incomingCallbackDto != null)
            {
                AnswerCallbackQueryDto answerQuery = new AnswerCallbackQueryDto {
                    callback_query_id = incomingCallbackDto.Id.ToString(),
                    text = incomingCallbackDto.Data
                    //Sample Ожидание... или Ваши данные переданы
                };

                await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(answerQuery));
            
                var response = await _telegramRequest.SendCallback(answerQuery);
            }
            return base.Checker(incomingCallbackDto);
        }
    }
}