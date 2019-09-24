using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class ResponseDto
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        [JsonProperty("result")]
        public ResultDto Result { get; set; }
        public System.DateTime DateRecieved { get; set; }
        public ResponseDto() => DateRecieved = System.DateTime.Now;
    }
}