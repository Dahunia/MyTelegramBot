using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class MessageDto//<TButton> where TButton : class
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; }
        [JsonProperty("from")]
        public FromDto From { get; set; }
        [JsonProperty("chat")]
        public ChatDto Chat { get; set; }
        [JsonProperty("date")]
        public ulong Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        public System.DateTime MessageSent { get; set; }
        public MessageDto() =>
            MessageSent = System.DateTime.Now;
    }
}