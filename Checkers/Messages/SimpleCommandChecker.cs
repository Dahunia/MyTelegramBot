using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyTelegramBot.Interface;
using MyTelegramBot.Web;
using MyTelegramBot.Dtos.Markets.Binance;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;
using MyTelegramBot.Helpers;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Log;

namespace MyTelegramBot.Checkers.Messages
{
    public class SimpleCommandChecker : AbstractMessageChecker
    {
        private const string Binance24hrUrl = "https://api.binance.com/api/v1/ticker/24hr?symbol=pair";
        private IMyLogger<AbstractMessageChecker> _logger;
        private readonly string[] commands = 
            {"/start", "/remove", "/inline", "cat"};
        private readonly ITelegramView _view;
        public SimpleCommandChecker(
            IMyLogger<AbstractMessageChecker> logger,
            IBackwardRepository backwardRepository,
            ITelegramRequest telegramRequest)
        : base(logger, backwardRepository, telegramRequest)
        {
            _logger = logger;
        }
        public override async Task<string> Checker(MessageDto incomingMessageDto)
        {
            var userDto = incomingMessageDto.From;

            if (incomingMessageDto.Text != "")
            {
                var otherView = await CreateMessageForSend(incomingMessageDto);
                return await base.SendView(otherView, incomingMessageDto);
            }
            return await base.Checker(incomingMessageDto);
        }
        public async Task<MessageForSendDto<object>> CreateMessageForSend(MessageDto message) 
        {
            MessageForSendDto<object> messageForSend;
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            switch (messageText.ToLower()) {
                case "/start":
                    messageForSend = new MessageForSendDto<object>() {
                            ChatId = chatId,
                            Text = "Наберите валютную пару Binance или " +
                                "выберите из примерных предложенных.",
                            ReplyMarkup = GetButtons(chatId)
                        };
                    break;
                case "/remove":
                    messageForSend = new MessageForSendDto<object>() {
                        ChatId = chatId,
                        Text = "Удаление клавиатуры",
                        ReplyMarkup = new TelegramRemoveButtons()
                    };
                    break;
                case "/inline":
                   messageForSend = new MessageForSendDto<object>() {
                        ChatId = chatId,
                        Text = "inline menu",
                        ReplyMarkup = GetInlineButtons(chatId)
                    };
                    break;
                default:
                    string symbol = messageText.ToUpper();
                    string url = Binance24hrUrl.Replace("pair", symbol);
                    messageForSend = new MessageForSendDto<object>() {
                        ChatId = chatId
                    };
                    try {        
                        var apiLogger = _logger as MyLogger<ApiGetingData<_24hrTickerDto>>;
                        var get24hrTicker = new ApiGetingData<_24hrTickerDto>(apiLogger);
                        var ticker = await get24hrTicker.GetDataAsync(url);

                        messageForSend.Text = ticker.ToString();

                        await _logger.LogInformation(ticker.ToString());
                    } catch(Exception ex) {
                        await _logger.LogInformation(ex.Message);
                        messageForSend.Text  = $"Пары на Binance {messageText} не существует";
                    }
                    break;
            }
            return messageForSend;
        }
        private TelegramButtons GetButtons(long chat_id)
        {
            List<string> keyboardButton1 = new List<string>() { "BNBUSDT", "BNBBTC", "BNBETH" };
            List<string> keyboardButton2 = new List<string>() { "BTCUSDT", "ETHUSDT", "LTCUSDT" };
            List<List<string>> keyboard = new List<List<string>>() {
                keyboardButton1,
                keyboardButton2
            };
            //return JsonConvert.SerializeObject(new TelegramButtons(keyboard));
            return new TelegramButtons(keyboard);
        }
        private InlineKeyboardMarkup GetInlineButtons(long chat_id) {
            var key1 = new InlineKeyboardButton("url1_1", "", "ya.ru");
            var key2 = new InlineKeyboardButton("url1_2", "", "yandex.ru");
            List<InlineKeyboardButton> keyboardButtons = new List<InlineKeyboardButton>() {
                key1,
                key2
            };
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>() {
                keyboardButtons
            };
            var inlineMarkup = new InlineKeyboardMarkup(buttons);

            //return JsonConvert.SerializeObject(inlineMarkup);
            return inlineMarkup;
        }
    }
}