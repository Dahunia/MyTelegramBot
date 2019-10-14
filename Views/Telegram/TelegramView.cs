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
        private readonly IMyLogger<TelegramView> _logger;
        //private readonly IAuthRepository _authRepository;
        private readonly IDataRepository _dataRepository;
        private readonly IBackwardRepository _backwardRepository;
        private readonly ITelegramRequest _telegramRequest;

        public TelegramView(
            IMyLogger<TelegramView> logger,
            //IAuthRepository authRepository,
            IBackwardRepository backwardRepository,
            IDataRepository dataRepository)
        {            
            //_authRepository = authRepository;
            _dataRepository = dataRepository;
            _logger = logger;
        }

        public async Task<MessageForSendDto<InlineKeyboardMarkup>> StartView(MessageDto messageDto)
        {
            var languageCode = messageDto.From.LanguageCode;
            var inlineKeyboardMarkup = await CategoriesMenu(languageCode);
            inlineKeyboardMarkup.AddLineButtons(await GetMainButtons());

            var messageForSend = await CreateMessageForSend(messageDto, inlineKeyboardMarkup);
            messageForSend.Text = $"All categories";
            return messageForSend;
        }
        public async Task<MessageForSendDto<InlineKeyboardMarkup>> DefaultView(MessageDto messageDto)
        {
            var languageCode = messageDto.From.LanguageCode;
            var inlineKeyboardMarkup = await CategoriesMenu(languageCode);
            inlineKeyboardMarkup.AddLineButtons(await GetMainButtons());

            var messageForSend = await CreateMessageForSend(messageDto, inlineKeyboardMarkup);
            messageForSend.Text = messageForSend.Text = $"Вы ввели: {messageDto.Text}, выберите пункт меню.";
            return messageForSend;
        }
        public async Task<MessageTextForEditDto> DefaultCallbackView(MessageDto messageDto)
        {
            var messageTextForEditDto = 
                await CreateMessageTextForEdit(messageDto);

            messageTextForEditDto.Text = $"Выберите пункт меню / Select a menu item";
    
            return messageTextForEditDto;
        }
        public async Task<MessageTextForEditDto> CategoriesForEditView(MessageDto messageDto, int categoryId)
        {
            var languageCode = messageDto.From.LanguageCode;

            var messageTextForEditDto = 
                await CreateMessageTextForEdit(messageDto);

            messageTextForEditDto.Text = 
                (await _dataRepository.GetCategory(categoryId))?.Name ?? "Нет подкатегорий";
 
            var line = await CategoriesMenu(languageCode, categoryId);
            line.AddLineButtons(await GetMainButtonsWithBackward());

            messageTextForEditDto.ReplyMarkup = line;

            return messageTextForEditDto;
        }
        public async Task<MessageTextForEditDto> ProductsForEditView(
            MessageDto messageDto, int categoryId)
        {
            var languageCode = messageDto.From.LanguageCode;

            var messageTextForEditDto = await CreateMessageTextForEdit(messageDto);

            var product = await _dataRepository.GetFirstProduct();
            
            var line = await ProductsInlineKeybaord(categoryId, languageCode);
            line.AddLineButtons( await GetMainButtonsWithBackward());

            messageTextForEditDto.ReplyMarkup = line;

            return messageTextForEditDto;
        }
        public async Task<MessageTextForEditDto> QuestionView(MessageDto messageDto)
        {
            var messageTextForEditDto = 
                await CreateMessageTextForEdit(messageDto);
            messageTextForEditDto.Text = "If you have questions, write me to jadarya@mail.ru";
           
            var settingKeyboard = messageTextForEditDto.ReplyMarkup.inline_keyboard.LastOrDefault();
            var backward = GetBackwardButton();
            
            if (!settingKeyboard.Contains(backward))
            {
                settingKeyboard.Add(GetBackwardButton());
            }
        
            return messageTextForEditDto;
        }

        public async Task<MessageTextForEditDto> AboutView(MessageDto messageDto)
        {
            var messageTextForEditDto = 
                await CreateMessageTextForEdit(messageDto);
            messageTextForEditDto.Text = "My name is Dasha. My mail is jadarya@mail.ru";

            return messageTextForEditDto;
        }
        public async Task<InlineKeyboardMarkup> CategoriesMenu(
            string languageCode, int categoryId = 0) =>
            await Task.Run(async () => {
                var categories = await _dataRepository.GetCategories(categoryId, languageCode);
                var lineButtons = new List<InlineKeyboardButton>();
                var inlineKeyboardMarkup = new InlineKeyboardMarkup();
                
                int num = 0;
                foreach (var category in categories.Select(c => new {c.Name, c.Id}))
                {
                    inlineKeyboardMarkup.AddButton(
                        new InlineKeyboardButton(category.Name, null, @"/cat" + category.Id.ToString()),
                        num++ / 2
                    );
                }
                //inlineKeyboardMarkup.AddLineButtons(await GetMainButtons());
                return inlineKeyboardMarkup;
            });

        public async Task<InlineKeyboardMarkup> ProductsInlineKeybaord(
            int categoryId, string languageCode) =>
            await Task.Run(async () => {
                var products = (await _dataRepository.GetProducts(categoryId, languageCode))
                    .Select(p => new { p.Id, p.Name, p.Price }).ToList();
                var lineButtons = new List<InlineKeyboardButton>();
                var inlineKeyboardMarkup = new InlineKeyboardMarkup();
                
                int num = 0;
                foreach (var product in products)
                {
                    inlineKeyboardMarkup.AddButton(
                        new InlineKeyboardButton(product.Name, null, @"/product" + product.Id.ToString()),
                        num++ / 2
                    );
                }
                //inlineKeyboardMarkup.AddLineButtons(await GetMainButtons());
                return inlineKeyboardMarkup;
            });

        public async Task<List<InlineKeyboardButton>> GetMainButtons() =>
            await Task.Run(() => {
                return  new List<InlineKeyboardButton>()
                {   
                    new InlineKeyboardButton("Есть вопрос?", null, @"/?"),
                    new InlineKeyboardButton("О нас", null, @"/about"),
                };
            });
        
        public async Task<List<InlineKeyboardButton>> GetMainButtonsWithBackward() =>
            await Task.Run(async () => {
                var line = await GetMainButtons();
                line.Add(GetBackwardButton());
                return line;
            });
            
        public InlineKeyboardButton GetBackwardButton() =>
             new InlineKeyboardButton("Назад", null, @"/backward");
        private async Task<MessageForSendDto<TButton>> CreateMessageForSend<TButton>(MessageDto message, TButton replyMarkup = null) 
            where TButton: class =>
            await Task.Run(() => 
                {
                    return new MessageForSendDto<TButton>() {
                        ChatId = message.Chat.Id, //290563423,//message.Chat.Id,
                        Text = "Text default",
                        ReplyMarkup = replyMarkup
                    };
                }); 

        private async Task<MessageTextForEditDto> CreateMessageTextForEdit(MessageDto messageDto) =>
            await Task.Run(() => {
                return new MessageTextForEditDto() {
                    ChatId = messageDto.Chat.Id,//290563423,
                    MessageId = messageDto.Id,
                    ReplyMarkup = messageDto.ReplyMarkup,
                    Text = "default text for change"
                };
            });
    }
}