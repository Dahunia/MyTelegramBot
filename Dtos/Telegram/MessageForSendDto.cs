using MyTelegramBot.Helpers;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class MessageForSendDto<TButton> where TButton : class
    {
        [JsonProperty("chat_id")]
        public long ChatId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        //public TelegramButtons reply_markup { get; set; }
        //public string reply_markup { get; set; }
        [JsonProperty("reply_markup")]
        public TButton ReplyMarkup { get; set; }
    }
}