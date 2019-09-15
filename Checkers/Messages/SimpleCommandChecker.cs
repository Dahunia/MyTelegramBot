using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Interface;
using MyTelegramBot.Data.Work;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Dtos.Markets.Binance;
using MyTelegramBot.Dtos.Telegram;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;
using MyTelegramBot.Helpers;

namespace MyTelegramBot.Checkers.Messages
{
    public class SimpleCommandChecker : AbstractChecker
    {
        private const string Binance24hrUrl = "https://api.binance.com/api/v1/ticker/24hr?symbol=pair";
        private readonly string[] commands = 
            {"/start", "/remove", "/inline", "/cat"};
        public SimpleCommandChecker(
            ILoggerFactory loggerFactory, 
            IMyLogger filelogger, 
            ITelegramApiRequest telegramRequest) 
            : base(loggerFactory, filelogger, telegramRequest)
        {}

        public override async Task<object> Checker(IncomingRequestDto incomingRequest)
        {
            var incommingMessageDto = incomingRequest.message;
            if (incommingMessageDto != null 
                && 
                commands.Contains(incommingMessageDto?.Text))
            {
                var messageForSend = await CreateMessageForSend(incommingMessageDto);

                await LogInformation("RESPONSE TO USER\n" + messageForSend.GetDump());
                
                var response = await _telegramRequest.SendMessage(messageForSend);

                return response;
            }
            return base.Checker(incomingRequest);
        }

         private async Task<MessageForSendDto> CreateMessageForSend(MessageDto message) {
            var chatId = message.Chat.Id;
            var messageText = message.Text;
            var messageForSend = new MessageForSendDto() {
                chat_id = chatId
            };
            switch (messageText.ToLower()) {
                case "/start":
                    messageForSend.text = "Наберите валютную пару Binance или " +
                        "выберите из примерных предложенных.";
                    //messageForSend.reply_markup = GetButtons(chatId);
                    break;
                case "/remove":
                    messageForSend.text = "Удаление клавиатуры";
                    //messageForSend.reply_markup = JsonConvert.SerializeObject(new TelegramRemoveButtons());
                    break;
                case "/inline":
                    messageForSend.text = "inline menu";
                    messageForSend.reply_markup = GetInlineButtons(chatId);
                    break;
                default:
                    string symbol = messageText.ToUpper();
                    string url = Binance24hrUrl.Replace("pair", symbol);
                    try {        
                        var get24hrTicker = new ApiGetingData<_24hrTickerDto>(_logger, _filelogger);
                        var ticker = await get24hrTicker.GetDataAsync(url);

                        messageForSend.text = ticker.ToString();

                        await LogInformation(ticker.ToString());
                    } catch(Exception ex) {
                        await LogInformation(ex.Message);

                        messageForSend.text = $"Пары на Binance {messageText} не существует";
                    }
                    break;
            }
            return messageForSend;
        }

        private string GetButtons(long chat_id)
        {
            List<string> keyboardButton1 = new List<string>() { "BNBUSDT", "BNBBTC", "BNBETH" };
            List<string> keyboardButton2 = new List<string>() { "BTCUSDT", "ETHUSDT", "LTCUSDT" };
            List<List<string>> keyboard = new List<List<string>>() {
                keyboardButton1,
                keyboardButton2
            };
            return JsonConvert.SerializeObject(new TelegramButtons(keyboard));
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