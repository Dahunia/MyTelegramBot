using Newtonsoft.Json;

namespace MyTelegramBot.Dtos.Telegram
{
    public class ResponseDto
    {
        public long Id { get; set; }
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        [JsonProperty("result")]
        public ResultDto Result { get; set; }
        public System.DateTime DateSent { get; set; }
        public ResponseDto() => DateSent = System.DateTime.Now;
    }
}