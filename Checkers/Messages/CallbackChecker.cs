using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Dtos.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Checkers.Messages
{
    public class CallbackChecker : AbstractChecker
    {
        public CallbackChecker(
            ILoggerFactory loggerFactory, 
            IMyLogger filelogger, 
            ITelegramApiRequest telegramRequest) 
            : base(loggerFactory, filelogger, telegramRequest) 
        {}

        public override async Task<object> Checker(IncomingRequestDto incomingRequest)
        {
            var callbackQuery = incomingRequest.callback_query;

            if (callbackQuery != null)
            {
                AnswerCallbackQueryDto answerQuery = new AnswerCallbackQueryDto {
                    callback_query_id = callbackQuery.id,
                    text = callbackQuery.data
                    //Sample Ожидание... или Ваши данные переданы
                };

                await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(answerQuery));
            
                var response = await _telegramRequest.SendCallback(answerQuery);
            }
            return base.Checker(incomingRequest);
        }
    }
}