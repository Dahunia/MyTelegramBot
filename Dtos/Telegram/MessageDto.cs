using System.Collections.Generic;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class MessageDto//<TButton> where TButton : class
    {
        [JsonProperty("message_id")]
        public long Id { get; set; }
        [JsonProperty("from")]
        public FromDto From { get; set; }
        [JsonProperty("chat")]
        public ChatDto Chat { get; set; }
        [JsonProperty("date")]
        public System.UInt64 Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("entities")]
        public ICollection<EntityDto> Entities { get; set; }
        [JsonProperty("reply_markup")]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
        public System.DateTime MessageReceived { get; set; }
        public MessageDto() =>
            MessageReceived = System.DateTime.Now;
    }
}