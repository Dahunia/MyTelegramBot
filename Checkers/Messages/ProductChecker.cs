using System;
using System.Threading.Tasks;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Checkers.Messages
{
    public class ProductChecker : AbstractMessageChecker
    {
        private readonly string[] commands = {"/product", "/cat"};
        private readonly IDataRepository _repo;
        public ProductChecker(IDataRepository repo, IServiceProvider provider)
            : base(provider) =>
            _repo = repo;

        public override async Task<string> Checker(MessageDto incomingMessageDto) 
        {
            switch(incomingMessageDto?.Text.ToLower()) 
            {
                case "/cat":
                    var messageForSend = await CreateMessageForSend(incomingMessageDto);   
                    var response = await _telegramRequest.SendMessage(messageForSend);

                    await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(messageForSend));
                    return response;
            }
            return await base.Checker(incomingMessageDto);
        }
        private async Task<MessageForSendDto<InlineKeyboardMarkup>> CreateMessageForSend(MessageDto message)
        {
             var messageForSend = new MessageForSendDto<InlineKeyboardMarkup>() {
                chat_id = message.Chat.Id
            };
            switch (message.Text.ToLower()) {
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