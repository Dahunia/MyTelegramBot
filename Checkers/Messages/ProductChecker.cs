using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Checkers.Messages
{
    public class ProductChecker : AbstractChecker
    {
        private readonly string[] commands = {"/product", "/cat"};
        private readonly IDataRepository _repo;
        public ProductChecker(
            ILoggerFactory loggerFactory, 
            IMyLogger filelogger, 
            ITelegramApiRequest telegramRequest,
            IDataRepository repo) 
            : base(loggerFactory, filelogger, telegramRequest) =>
            _repo = repo;

        public override async Task<object> Checker(IncomingRequestDto incomingRequestDto) 
        {
            var incomingMessageDto = incomingRequestDto.message;
            if (incomingMessageDto != null
                && 
                incomingMessageDto.text == "/product")
            {

                var messageForSend = await CreateMessageForSend(incomingMessageDto);

                await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(messageForSend));
                
                var response = await _telegramRequest.SendMessage(messageForSend);

                return response;
            }
            return base.Checker(incomingRequestDto);
        }
        private async Task<MessageForSendDto> CreateMessageForSend(Message message)
        {
             var messageForSend = new MessageForSendDto() {
                chat_id = message.chat.id
            };
            switch (message.text.ToLower()) {
                case "/cat":
                    messageForSend.text = "Категории";
                    
                    var inlineKeyboard = new InlineKeyboardMarkup();
                    
                    //messageForSend.reply_markup = 
                    //    GetInlineButtons(message.chat.id);
                    break;
            }
            return messageForSend;
        }
    }
}