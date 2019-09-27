using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class CallbackQueryDto
    {
        //public long id { get; set; }
        [JsonProperty("id")]
        public long Id { get; set;}
        [JsonProperty("from")]
         public FromDto From { get; set; }
        [JsonProperty("message")]
        public MessageDto Message { get; set; }
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}