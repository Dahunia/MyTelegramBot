using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class ResultDto
    {
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }
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
    }
}