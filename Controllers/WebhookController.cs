using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyTelegramBot.Data.Work;
using MyTelegramBot.Dtos.Markets.Binance;
using MyTelegramBot.Data.Interface;
using System;
using Microsoft.Extensions.Options;
using MyTelegramBot.Data.Work.Interface;
using MyTelegramBot.Models.Settings;
using MyTelegramBot.Models.Telegram;
using System.Collections.Generic;
using Newtonsoft.Json;
using MyTelegramBot.Helpers;
using MyTelegramBot.Dtos.Telegram;
using System.Text.RegularExpressions;

namespace MyTelegramBot.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public const string Binance24hrUrl = "https://api.binance.com/api/v1/ticker/24hr?symbol=pair";
        private readonly ILogger<WebhookController> _logger;
        private readonly IMyLogger _filelogger;
        private readonly TelegramSettings _telegramConfig;
        private readonly ITelegramApiRequest _telegramRequest;
        public WebhookController(
            ILogger<WebhookController> logger, 
            IMyLogger filelogger,
            IOptions<TelegramSettings> telegramConfig,
            ITelegramApiRequest telegramRequest)
        {
            _logger = logger;
            _filelogger = filelogger;
            _telegramRequest = telegramRequest;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IncomingRequestDto incomingRequestDto)
        {
            await LogInformation("INPUT REQUEST \n" + HttpContext.Request.ReadRequestBody());

            if (incomingRequestDto.message != null)
                await AnswerIsMessage(incomingRequestDto.message);

            if (incomingRequestDto.callback_query != null)
                await AnswerIsCallbackQuery(incomingRequestDto.callback_query);

            return StatusCode(201);
        }
        private async Task AnswerIsCallbackQuery(Callback_Query callback_query)
        {
            AnswerCallbackQueryDto answerQuery = new AnswerCallbackQueryDto {
                callback_query_id = callback_query.id,
                text = callback_query.data
                //Sample Ожидание... или Ваши данные переданы
            };

            await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(answerQuery));
            
            var response = await _telegramRequest.AnswerCallbackQuery(answerQuery);   
        }

        private async Task AnswerIsMessage(Message message) 
        {       
            var messageForSend = await CreateMessageForSend(message);

            await LogInformation("RESPONSE TO USER\n" + JsonConvert.SerializeObject(messageForSend));
            
            var response = await _telegramRequest.SendMessage(messageForSend);   
        }
        private async Task<MessageForSendDto> CreateMessageForSend(Message message) {
            var messageForSend = new MessageForSendDto() {
                chat_id = message.chat.id
            };
            switch (message.text.ToLower()) {
                case "/start":
                    messageForSend.text = "Наберите валютную пару Binance или " +
                        "выберите из примерных предложенных.";
                    messageForSend.reply_markup = GetButtons(message.chat.id);
                    break;
                case "/remove":
                    messageForSend.text = "Удаление клавиатуры";
                    messageForSend.reply_markup = JsonConvert.SerializeObject(new TelegramRemoveButtons());
                    break;
                case "/inline":
                    messageForSend.text = "inline menu";
                    messageForSend.reply_markup = GetInlineButtons(message.chat.id);
                    break;
                default:
                    string symbol = message.text.ToUpper();
                    string url = Binance24hrUrl.Replace("pair", symbol);
                    try {        
                        var get24hrTicker = new ApiGetingData<_24hrTickerDto>(_logger, _filelogger);
                        var ticker = await get24hrTicker.GetDataAsync(url);

                        messageForSend.text = ticker.ToString();

                        await LogInformation(ticker.ToString());
                    } catch(Exception ex) {
                        await LogInformation(ex.Message);

                        messageForSend.text = $"Пары на Binance {message.text} не существует";
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
        private string GetInlineButtons(long chat_id) {
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

            return JsonConvert.SerializeObject(inlineMarkup);
        }

        public async Task LogInformation(string message) 
        {
            _logger.LogInformation(message);
            await _filelogger.WriteInformationAsync(message);
        }
    }
}