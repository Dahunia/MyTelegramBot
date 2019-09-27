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
        public override async Task<string> Checker(CallbackQueryDto incomingCallbackDto)
        {
            if (incomingCallbackDto != null)
            {
                var answerForSend = CreateAnswerForSend(incomingCallbackDto);                         
                var response = await _telegramRequest.ChangeMessage(answerForSend);

                await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(answerForSend));
                return response;
            }
            return await base.Checker(incomingCallbackDto);
        }

        private MessageTextForEditDto CreateAnswerForSend(CallbackQueryDto callbackQuery)
        {
            string answer = callbackQuery.Data;
            var chatId = callbackQuery.Message.Chat.Id;
            var replyMarkup = callbackQuery.Message.ReplyMarkup;
            var answerForSend = new MessageTextForEditDto {
                ChatId = chatId,
                MessageId = callbackQuery.Message.Id,
                Text = answer,
                ReplyMarkup = replyMarkup
            };
            /* var answerQuery = new AnswerCallbackQueryDto 
            {
                    callback_query_id = callbackQuery.Id,
                    text = callbackQuery.Data
                    //Sample Ожидание... или Ваши данные переданы
            }; */
            switch(callbackQuery.Data)
            {
                default:
                    break;
            }
            return answerForSend;
        }
    }
}