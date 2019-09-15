using MyTelegramBot.Helpers;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class MessageForSendDto<TButton> where TButton : class
    {
        public long chat_id { get; set; }
        //[JsonConverter(typeof(PlainJsonStringConverter))]
        public string text { get; set; }
        //public TelegramButtons reply_markup { get; set; }
        //public string reply_markup { get; set; }
        public TButton reply_markup { get; set; }
    }
}