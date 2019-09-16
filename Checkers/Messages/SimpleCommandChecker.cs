using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Data.Work;
using MyTelegramBot.Dtos.Markets.Binance;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;
using MyTelegramBot.Helpers;

namespace MyTelegramBot.Checkers.Messages
{
    public class SimpleCommandChecker : AbstractMessageChecker
    {
        private const string Binance24hrUrl = "https://api.binance.com/api/v1/ticker/24hr?symbol=pair";
        private readonly string[] commands = 
            {"/start", "/remove", "/inline", "/cat"};
        private readonly IDataRepository _repo;
        public SimpleCommandChecker(
            IDataRepository repo,
            IServiceProvider provider)//ILoggerFactory loggerFactory, IMyLogger filelogger, ITelegramApiRequest telegramRequest) 
            : base(provider)
            => _repo = repo;
        public override async Task<object> Checker(MessageDto incomingMessageDto)
        {
            var userDto = incomingMessageDto.From;

            if (commands.Contains(incomingMessageDto?.Text))
            {
                var messageForSend = await CreateMessageForSend(incomingMessageDto);
                var response = await _telegramRequest.SendMessage(messageForSend);
              
                await LogInformation("RESPONSE TO USER\n" + messageForSend.GetDump());
                await LogInformation("RESPONSE OF REQUEST" + JsonConvert.DeserializeObject(response).GetDump());

                return response;
            }
            return base.Checker(incomingMessageDto);
        }
        private async Task<MessageForSendDto<object>> CreateMessageForSend(MessageDto message) 
        {
            MessageForSendDto<object> messageForSend;
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            switch (messageText.ToLower()) {
                case "/start":
                    messageForSend = new MessageForSendDto<object>() {
                            chat_id = chatId,
                            text = "Наберите валютную пару Binance или " +
                                "выберите из примерных предложенных.",
                            reply_markup = GetButtons(chatId)
                        };
                    break;
                case "/remove":
                    messageForSend = new MessageForSendDto<object>() {
                        chat_id = chatId,
                        text = "Удаление клавиатуры",
                        reply_markup = new TelegramRemoveButtons()
                    };
                    break;
                case "/inline":
                   messageForSend = new MessageForSendDto<object>() {
                        chat_id = chatId,
                        text = "inline menu",
                        reply_markup = GetInlineButtons(chatId)
                    };
                    break;
                default:
                    string symbol = messageText.ToUpper();
                    string url = Binance24hrUrl.Replace("pair", symbol);
                    messageForSend = new MessageForSendDto<object>() {
                        chat_id = chatId
                    };
                    try {        
                        var get24hrTicker = new ApiGetingData<_24hrTickerDto>(_logger, _filelogger);
                        var ticker = await get24hrTicker.GetDataAsync(url);

                        messageForSend.text = ticker.ToString();

                        await LogInformation(ticker.ToString());
                    } catch(Exception ex) {
                        await LogInformation(ex.Message);

                        messageForSend.text  = $"Пары на Binance {messageText} не существует";
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