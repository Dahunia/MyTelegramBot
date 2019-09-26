using System;
using System.Threading.Tasks;
using MyTelegramBot.Interface;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using System.Collections.Generic;
using MyTelegramBot.Helpers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MyTelegramBot.Checkers.Messages
{
    public class ProductChecker : AbstractMessageChecker
    {
        private readonly string[] commands = {"/product", "/cat"};
        private readonly IDataRepository _repo;
        private readonly IAuthRepository _authRepository;
        public ProductChecker(
            ILogger<ProductChecker> logger,
            IMyLogger filelogger,
            ITelegramApiRequest telegramApiRequest,
            IDataRepository repo,
            IAuthRepository authRepository)
            : base(logger, filelogger, telegramApiRequest) =>
            (_repo, _authRepository) = (repo, authRepository);

        public override async Task<string> Checker(MessageDto incomingMessageDto) 
        {
            if (commands.Contains(incomingMessageDto.Text.ToLower()))
            {
                var messageForSend = await CreateMessageForSend(incomingMessageDto);
                var response = await _telegramRequest.SendMessage(messageForSend);
              
                await LogInformation("Was SENT TO USER\n" + messageForSend.GetDump());

                return response;
            }
            return await base.Checker(incomingMessageDto);
        }

        public async Task<MessageForSendDto<object>> CreateMessageForSend(MessageDto messageDto)
        {
            MessageForSendDto<object> messageForSend = null;
            var userId = messageDto.From.Id;
            var languageCode = messageDto.From.LanguageCode;
            var chatId = messageDto.Chat.Id;
            //var user = await _authRepository.GetUser(userId);
            switch(messageDto.Text.ToLower()) 
            {
                case "/cat":
                    var categories = await _repo.GetCategories(languageCode);
                    var lineButtons = new List<InlineKeyboardButton>();
                    var inlineKeyboardMarkup = new InlineKeyboardMarkup();
                    
                    int num = 0;
                    foreach (var category in categories.Select(c => c.Name))
                    {
                        inlineKeyboardMarkup.AddButton(
                            new InlineKeyboardButton(category, null, "sample.ru"),
                            num++ / 2
                        );
                    }

                    messageForSend = new MessageForSendDto<object>() {
                        ChatId = chatId,
                        Text = "Все категории",
                        ReplyMarkup = inlineKeyboardMarkup
                    }; 
                    break;
            }
            return messageForSend;
        }
    }
}