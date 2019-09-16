using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Dtos.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Checkers.Callback
{
    public class CallbackChecker : AbstractCallbackChecker
    {
        public CallbackChecker(IServiceProvider provider)//ILoggerFactory loggerFactory, IMyLogger filelogger, ITelegramApiRequest telegramRequest) 
            : base(provider) //loggerFactory, filelogger, telegramRequest
        {}
        public override async Task<object> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (incomingCallbackDto != null)
            {
                AnswerCallbackQueryDto answerQuery = new AnswerCallbackQueryDto {
                    callback_query_id = incomingCallbackDto.id,
                    text = incomingCallbackDto.data
                    //Sample Ожидание... или Ваши данные переданы
                };

                await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(answerQuery));
            
                var response = await _telegramRequest.SendCallback(answerQuery);
            }
            return base.Checker(incomingCallbackDto);
        }
    }
}