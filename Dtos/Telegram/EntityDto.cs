using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class EntityDto
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("length")]
        public int Length { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}