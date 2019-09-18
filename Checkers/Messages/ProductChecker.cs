using System;
using System.Threading.Tasks;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using System.Collections.Generic;
using MyTelegramBot.Helpers;

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
                    var categories = await _repo.GetCategories(incomingMessageDto.From.Id);
                    var lineButtons = new List<InlineKeyboardButton>();
                    var inlineKeyboardMarkup = new InlineKeyboardMarkup();
                    
                    int num = -1;
                    foreach (var category in categories)
                    {
                        inlineKeyboardMarkup.AddButton(
                            new InlineKeyboardButton(category.Name),
                            num++ / 2
                        );
                    }

                    var messageForSend = new MessageForSendDto<InlineKeyboardMarkup>() {
                        ChatId = incomingMessageDto.Chat.Id,
                        Text = "Все категории",
                        ReplyMarkup = inlineKeyboardMarkup
                    }; 

                    var response = await _telegramRequest.SendMessage(messageForSend);

                    await LogInformation("SENT TO USER\n" + messageForSend.GetDump());
                    
                    return response;
            }
            return await base.Checker(incomingMessageDto);
        }
    }
}