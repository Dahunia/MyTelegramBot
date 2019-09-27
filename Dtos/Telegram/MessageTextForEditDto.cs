using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class MessageTextForEditDto
    {
        [JsonProperty("chat_id")]
        public long ChatId { get; set; }
        [JsonProperty("message_id")]
        public long MessageId { get; set; }
        [JsonProperty("inline_message_id")]
        //optional. Required if chat_id and message_id is not specified.
        public string InlineMessageId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set;} 
        [JsonProperty("parse_mode")]
        //Send Markdown or HTML, if you want show bold, italic...
        public string ParseMode { get; set; }
        [JsonProperty("disable_web_page_preview")]
        public bool DisableWebPagePreview { get; set; }
        [JsonProperty("reply_markup")]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
    }
}