using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Interface;
using MyTelegramBot.Models.Telegram;

namespace MyTelegramBot.Views.Telegram
{
    public class TelegramView : ITelegramView
    {
        private readonly IDataRepository _dataRepository;
        //private readonly IAuthRepository _authRepository;
        private readonly IMyLogger<TelegramView> _logger;

        public TelegramView(
            IDataRepository dataRepository, 
            //IAuthRepository authRepository, 
            IMyLogger<TelegramView> logger)
        {
            _dataRepository = dataRepository;
            //_authRepository = authRepository;
            _logger = logger;
        }

        public async Task<List<InlineKeyboardButton>> GetMainButtons()
        {
             return await Task.Run(() => {
                List<InlineKeyboardButton> line = new List<InlineKeyboardButton>()
                {   
                    new InlineKeyboardButton("Есть вопрос?", null, @"/?"),
                    new InlineKeyboardButton("О нас", null, @"/about")
                };
                return line;
            });
        }
        public async Task<MessageForSendDto<InlineKeyboardMarkup>> Start(MessageDto messageDto)
        {
            var languageCode = messageDto.From.LanguageCode;
            var categories = await _dataRepository.GetCategories(languageCode);
            var inlineKeyboardMarkup = await CategoriesMenu(languageCode);

            var messageForSend = await CreateMessageForSend(messageDto, inlineKeyboardMarkup);
            messageForSend.Text = $"All categories";
            return messageForSend;
        }
        public async Task<MessageForSendDto<InlineKeyboardMarkup>> Default(MessageDto messageDto)
        {
            var languageCode = messageDto.From.LanguageCode;
            var categories = await _dataRepository.GetCategories(languageCode);
            var inlineKeyboardMarkup = await CategoriesMenu(languageCode);

            var messageForSend = await CreateMessageForSend(messageDto, inlineKeyboardMarkup);
            messageForSend.Text = messageForSend.Text = $"Вы ввели: {messageDto.Text}, выберите пункт меню.";
            return messageForSend;
        }
        public async Task<MessageTextForEditDto> Category(MessageDto messageDto, int categoryId)
        {
            var messageTextForEditDto = 
                await CreateMessageTextForEditDot(messageDto);

            messageTextForEditDto.Text = 
                (await _dataRepository.GetCategory(categoryId))?.Name ?? "Нет подкатегорий";
    
            return messageTextForEditDto;
        }
        public async Task<MessageTextForEditDto> Question(MessageDto messageDto)
        {
            var messageTextForEditDto = 
                await CreateMessageTextForEditDot(messageDto);
            messageTextForEditDto.Text = "If you have questions, write me to jadarya@mail.ru";

            return messageTextForEditDto;
        }

        public async Task<MessageTextForEditDto> About(MessageDto messageDto)
        {
            var messageTextForEditDto = 
                await CreateMessageTextForEditDot(messageDto);
            messageTextForEditDto.Text = "My name is Dasha. My mail is jadarya@mail.ru";

            return messageTextForEditDto;
        }
        public async Task<InlineKeyboardMarkup> CategoriesMenu(string languageCode, int categoryId = 0)
        {
            var categories = await _dataRepository.GetCategories(languageCode, categoryId);
            var lineButtons = new List<InlineKeyboardButton>();
            var inlineKeyboardMarkup = new InlineKeyboardMarkup();
            
            int num = 0;
            foreach (var category in categories.Select(c => new {c.Name, c.Id}))
            {
                inlineKeyboardMarkup.AddButton(
                    new InlineKeyboardButton(category.Name, null, @"/" + category.Id.ToString()),
                    num++ / 2
                );
            }
            inlineKeyboardMarkup.AddLineButtons(await GetMainButtons());

            return inlineKeyboardMarkup;
        }
        private async Task<MessageForSendDto<TButton>> CreateMessageForSend<TButton>(MessageDto message, TButton replyMarkup = null) 
            where TButton: class
        {
            return await Task.Run(() => 
            {
                return new MessageForSendDto<TButton>() {
                    ChatId = message.Chat.Id,
                    Text = "Text default",
                    ReplyMarkup = replyMarkup
                };
            }); 
        }
        private async Task<MessageTextForEditDto> CreateMessageTextForEditDot(MessageDto messageDto)
        {
            return await Task.Run(() => {
                return new MessageTextForEditDto() {
                    ChatId = messageDto.Chat.Id,
                    MessageId = messageDto.Id,
                    ReplyMarkup = messageDto.ReplyMarkup,
                    Text = "default text for change"
                };
            });
        }
    }
}